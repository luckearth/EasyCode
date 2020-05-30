using EasyCode.EventBus.Abstractions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Framing.Impl;

namespace EasyCode.EventBusRabbitMQ.CommandBus
{
    public class RabbitMqCommandBusServer: ICommandBusServer
    {
        const string ExchangeName = "request-handler";
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private string _queueName;
        private IModel _model;
        private ICommandHandlerFactory _factory;
        private EventingBasicConsumer _consumer;
        private string reponseQueueName;
        public RabbitMqCommandBusServer(ICommandHandlerFactory factory,IRabbitMQPersistentConnection persistentConnection, string queueName = "request.handler.all")
        {
            reponseQueueName = queueName + Guid.NewGuid();
            _factory = factory;
            _persistentConnection = persistentConnection;
            _queueName = queueName;
            reponseQueueName = "response-handler";

        }

        public void Start()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }
            _model = _persistentConnection.CreateModel();
            _model.BasicQos(0, 2, true);//全局限定消费者每次处理的包，没有收到ACK 不下发

            _model.ExchangeDeclare(exchange: ExchangeName, type: "topic",false,false);
            _model.QueueDeclare(reponseQueueName, false, false, false);
            //_model.QueueBind(_queueName, ExchangeName, "request-handler-all");
            _consumer = new EventingBasicConsumer(_model);
            _consumer.Received += OnMessageReceivod;
            _model.BasicConsume(_queueName, false, Guid.NewGuid().ToString(), _consumer);
        }
        private void OnMessageReceivod(object sender, BasicDeliverEventArgs eventArgs)
        {
            Console.WriteLine("收到信息");
            var model = ((EventingBasicConsumer) sender).Model;
            ICommandHandler handler;
            try
            {
                model.BasicAck(eventArgs.DeliveryTag, false);
                handler = _factory.CreateCommandHandler(eventArgs.BasicProperties.Type);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                model.BasicAck(eventArgs.DeliveryTag,false);
                handler = null;
            }

            if (handler != null)
            {
                Task task=new Task(async () =>
                {
                    try
                    {
                        //Console.WriteLine("我开始执行");
                        var responseBuffer = await handler.HandleAsync(eventArgs.Body.ToArray());
                        var responseProperties = _model.CreateBasicProperties();
                        responseProperties.CorrelationId = eventArgs.BasicProperties.CorrelationId;
                        _model.BasicPublish("", eventArgs.BasicProperties.ReplyTo,responseProperties,responseBuffer);
                        //Console.WriteLine("");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        
                    }
                });
                task.Start();
            }
        }
        
    }
}
