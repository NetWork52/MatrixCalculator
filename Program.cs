using Grpc.Core;        // Для работы с gRPC
using Matrix;           // Пространство имен сгенерированного кода из файла exec.proto
using MatrixCalculator;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class MatrixServiceImpl : MatrixService.MatrixServiceBase
{
    private readonly TaskQueue _taskQueue;
    private readonly ResultQueue _resultQueue;

    public MatrixServiceImpl()
    {
        _taskQueue = new TaskQueue();
        _resultQueue = new ResultQueue();
    }

    public override async Task<MatrixResponse> PerformMatrixOperation(MatrixRequest request, ServerCallContext context)
    {
        // Кодируем запрос в JSON для передачи через RabbitMQ
        var taskMessage = JsonSerializer.Serialize(request);

        // Отправляем задачу в очередь
        _taskQueue.PublishTask(taskMessage);

        // Получаем результат из очереди
        var result = _resultQueue.GetResult();

        var response = new MatrixResponse();
        response.Result.AddRange(result.Split(',').Select(int.Parse));

        return await Task.FromResult(response);
    }
}

public class ResultQueue
{
    private readonly ConnectionFactory _factory;

    public ResultQueue()
    {
        _factory = new ConnectionFactory() { HostName = "localhost" };
    }

    public string GetResult()
    {
        using var connection = _factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "matrix_responses", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var result = "";
        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            result = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[x] Получен результат: {result}");
        };

        channel.BasicConsume(queue: "matrix_responses", autoAck: true, consumer: consumer);

        // Пауза, чтобы подождать получения сообщения
        Thread.Sleep(1000);

        return result;
    }
}

public class Program
{
    public static async Task Main(string[] args)    {
        // Создаем и настраиваем сервер
        var server = new Server
        {
            Services = { MatrixService.BindService(new MatrixServiceImpl()) },  // Регистрация сервиса
            Ports = { new ServerPort("localhost", 5000, ServerCredentials.Insecure) }  // Порт, на котором будет слушать сервер
        };

        // Запуск сервера
        server.Start();
        Console.WriteLine("Server listening on port 5000");


         // Запускаем клиента
        // var client = new Client("http://localhost:5000");
        var client = new FileInputClient("http://localhost:5000", "input.txt");
        await client.Run();


        Console.WriteLine("Нажмите Enter для завершения сервера...");
        Console.ReadLine();

        // Ожидаем завершения работы сервера
        await server.ShutdownAsync();
    }

    // static void Main(string[] args)
    // {
    //     var taskQueue = new TaskQueue();
    //     var message = "{\"Length\": 2, \"Width\": 2, \"Operation\": \"sum\", \"Data\": [1, 2, 3, 4, 5, 6, 7, 8]}";
    //     taskQueue.PublishTask(message);

    //     Console.WriteLine("Задача отправлена в очередь.");
    // }
}
