using RabbitMQ.Client;
using System;
using System.Text;
using RabbitMQ.Client.Events;

public class TaskQueue
{
    private readonly ConnectionFactory _factory;
    private readonly string _queueName = "matrix_tasks";
    private readonly string _responseQueueName = "matrix_responses";

    public TaskQueue()
    {
        _factory = new ConnectionFactory() { HostName = "localhost" };
    }

    public void PublishTask(string message)
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: "",
                             routingKey: _queueName,
                             basicProperties: null,
                             body: body);

        Console.WriteLine($"[x] Отправлено: {message}");
    }
     public string GetResult()
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _responseQueueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var result = "";
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            result = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[x] Получен результат: {result}");
        };

        channel.BasicConsume(queue: _responseQueueName,
                             autoAck: true,
                             consumer: consumer);

        // Пауза, чтобы подождать получения сообщения
        Thread.Sleep(1000);

        return result;
    }

}
