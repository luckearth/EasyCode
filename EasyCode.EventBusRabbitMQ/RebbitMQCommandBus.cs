using EasyCode.EventBus.Abstractions;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyCode.EventBusRabbitMQ
{
    public class RebbitMQCommandBus: ICommandBus
    {
        const string EXCHANGE_NAME = "easycode_event_bus";
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private string _queueName;
        private IModel _model;
        public RebbitMQCommandBus(IRabbitMQPersistentConnection persistentConnection, string queueName = "request-handle")
        {
            _persistentConnection = persistentConnection;
            _queueName = queueName;
            _model = persistentConnection.CreateModel();
            _model.BasicQos(0, 5, true);//全局限定消费者每次处理的包，没有收到ACK 不下发
            _model.ExchangeDeclare(exchange: EXCHANGE_NAME, type: "topic", false, false, null);
        }

        public Task<byte[]> GetResponse(string requestType, byte[] requestBuffer)
        {
            throw new NotImplementedException();
        }
    }
}
