using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Receive
{
    public class Receive
    {
        public static void Main()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5673,
                UserName = "teste",
                Password = "Teste2020!"
            };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "queue-products",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        try
                        {
                            var body = ea.Body;
                            var message = Encoding.UTF8.GetString(body.ToArray());

                            Console.WriteLine($" A mensagem '{message}' foi recebida com sucesso.");

                            channel.BasicAck(ea.DeliveryTag, false);
                        }
                        catch (Exception)
                        {
                            channel.BasicNack(ea.DeliveryTag, false, true);
                        }
                    };

                    channel.BasicConsume(queue: "queue-products",
                                         autoAck: false,
                                         consumer: consumer);

                    Console.WriteLine("Caso existam mensagens na fila, o console vai exibi-las,");
                    Console.WriteLine("ou conforme novas mensagens forem entrando na fila.");
                    Console.WriteLine("Caso queira fechar a aplicação, aperte [ENTER].");
                    Console.ReadLine();
                }
            }
        }
    }
}
