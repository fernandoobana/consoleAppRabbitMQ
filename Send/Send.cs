using System;
using RabbitMQ.Client;
using System.Text;

namespace Send
{
    public class Send
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

                    var close = "";
                    Console.WriteLine(" Digite sua mensagem e aperte [ENTER] para enviá-la para a fila");
                    Console.WriteLine(" ou digite 000 e aperte [ENTER] para sair.");

                    do
                    {
                        Console.WriteLine("Mensagem: ");
                        var message = Console.ReadLine();

                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "exc-product",
                                            routingKey: "queue-products",
                                            basicProperties: null,
                                            body: body);

                        Console.WriteLine($"A mensagem {message} foi enviada para a fila do RabbitMQ.");

                        Console.WriteLine(" Pressione [ENTER] para continuar enviando mensagens ou 000 para sair.");
                        close = Console.ReadLine();
                    } while (close != "000");
                }
            }

            Console.WriteLine(" [ENTER] para encerrar a aplicação.");
            Console.ReadLine();
        }
    }
}
