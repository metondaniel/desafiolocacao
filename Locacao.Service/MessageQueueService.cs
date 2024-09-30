using Locacao.Service.Entities;
using Locacao.Service.Interfaces.Services;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locacao.Service
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageQueueService()
        {
            var factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672, Password="guest",UserName="guest" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public async Task PublicarEventoMotoCadastrada(Moto moto)
        {
            _channel.QueueDeclare(queue: "moto_cadastrada", durable: false, exclusive: false, autoDelete: false, arguments: null);

            string mensagem = JsonConvert.SerializeObject(moto);
            var body = Encoding.UTF8.GetBytes(mensagem);

            _channel.BasicPublish(exchange: "", routingKey: "moto_cadastrada", basicProperties: null, body: body);
        }

        public void ConsumirEventosMotos2024()
        {
            _channel.QueueDeclare(queue: "moto_cadastrada", durable: false, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var mensagem = Encoding.UTF8.GetString(body);
                var moto = JsonConvert.DeserializeObject<Moto>(mensagem);

                if (moto.Ano == 2024)
                {
                    // Lógica para armazenar a mensagem no banco de dados
                    Console.WriteLine($"Moto 2024 cadastrada: {moto.Placa}");
                }
            };

            _channel.BasicConsume(queue: "moto_cadastrada", autoAck: true, consumer: consumer);
        }
    }

}
