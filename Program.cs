using Grpc.Core;        // Для работы с gRPC
using Matrix;           // Пространство имен сгенерированного кода из файла exec.proto
using MatrixCalculator;

public class MatrixServiceImpl : MatrixService.MatrixServiceBase
{
    // Реализация метода PerformMatrixOperation
    public override Task<MatrixResponse> PerformMatrixOperation(MatrixRequest request, ServerCallContext context)
    {
        var result = new List<int>();

        // Пример обработки запроса (сложение/умножение)
        if (request.Operation == OperationType.Addition) {
            // Сложение матриц
            result = MatrixOperations.AddMatrices(request.Length, request.Width, request.MatrixData.ToList());
        }
        else if (request.Operation == OperationType.Multiplication) {
            // Умножение матриц
            result = MatrixOperations.MultiplyMatrices(request.Length, request.Width, request.MatrixData.ToList());
        }
        else {
           throw new RpcException(new Status(StatusCode.InvalidArgument, "Неизвестная операция"));
        }

        // Возвращаем результат выполнения операции
        return Task.FromResult(new MatrixResponse { Result = { result } });
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        // Создаем и настраиваем сервер
        var server = new Server
        {
            Services = { MatrixService.BindService(new MatrixServiceImpl()) },  // Регистрация сервиса
            Ports = { new ServerPort("localhost", 5000, ServerCredentials.Insecure) }  // Порт, на котором будет слушать сервер
        };

        // Запуск сервера
        server.Start();

        Console.WriteLine("Server listening on port 5000");
        Console.ReadKey();

        // Ожидаем завершения работы сервера
        server.ShutdownAsync().Wait();
    }
}
