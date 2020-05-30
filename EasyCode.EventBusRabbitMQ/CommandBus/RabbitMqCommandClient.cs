using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EasyCode.EventBus.Abstractions;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EasyCode.EventBusRabbitMQ.CommandBus
{
    public class RabbitMqCommandClient : ICommandBusClient
    {
        const string ExchangeName = "request-handler";
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private string _queueName;
        private IModel _model;
        private AsyncEventingBasicConsumer _consumer;
        private ConcurrentDictionary<string, TaskCompletionSource<byte[]>> _responseDictionary;
        private string reponseQueueName;
        private ILogger _logger;
        public RabbitMqCommandClient(IRabbitMQPersistentConnection persistentConnection,ILogger<RabbitMqCommandClient> logger, string queueName = "request.handler.all")
        {
            reponseQueueName = "response-handler";
            _persistentConnection = persistentConnection;
            
            _queueName = queueName;
            _model = _persistentConnection.CreateModel();
            //_model.BasicQos(0,5,true);
            _model.QueueDeclare(reponseQueueName, false, false,false);
            _model.ExchangeDeclare(exchange: ExchangeName, type: "topic", false, false);
            _model.QueueDeclare(queueName, false, false, false);
            _model.QueueBind(queueName, ExchangeName, _queueName);
            _consumer=new AsyncEventingBasicConsumer(_model);
            _consumer.Received += OnMessageRecived;
            _model.BasicConsume(reponseQueueName, false, Guid.NewGuid().ToString(), _consumer);
            _responseDictionary=new ConcurrentDictionary<string, TaskCompletionSource<byte[]>>();
            _logger = logger;
        }

        private async Task OnMessageRecived(object sender, BasicDeliverEventArgs eventArgs)
        {
            var model = ((AsyncDefaultBasicConsumer) sender).Model;
            model.BasicAck(eventArgs.DeliveryTag,false);
            TaskCompletionSource<byte[]> tarray;
            if (_responseDictionary.TryRemove(eventArgs.BasicProperties.CorrelationId, out tarray))
            {
               await Task.Run(() => tarray.SetResult(eventArgs.Body.ToArray()));
            }
            
        }
        public Task<byte[]> GetResponseAsync(string requestType, byte[] requestBuffer)
        {
            string correlationId = Guid.NewGuid().ToString();
            TaskCompletionSource<byte[]> tcSource=new TaskCompletionSource<byte[]>();
            _responseDictionary[correlationId] = tcSource;
            try
            {
                var properties = _model.CreateBasicProperties();
                properties.CorrelationId = correlationId;
                properties.ReplyTo = reponseQueueName;
                properties.Type = requestType;
                properties.DeliveryMode = 2;
                //var model = _persistentConnection.CreateModel();
                _model.ExchangeDeclare(exchange: ExchangeName, type: "topic");
                _model.BasicPublish(ExchangeName, _queueName, properties, requestBuffer);
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message},{e.InnerException},{e.Source}");
            }
           
            return tcSource.Task;
        }
    }
}
