// ПРОВЕРИТЬ!!!!!!!! (ИСПРАВИТЬ)

using Grpc.Net.Client;
using Matrix;  // Пространство имен из сгенерированных классов (MatrixRequest, MatrixResponse)
using System;
using System.Linq;
using System.Threading.Tasks;

public class Client
{
    private readonly string _serverAddress;

    public Client(string serverAddress)
    {
        _serverAddress = serverAddress;
    }

    public async Task Run()
    {
        using var channel = GrpcChannel.ForAddress(_serverAddress);
        var client = new MatrixService.MatrixServiceClient(channel);

        Console.WriteLine("Выберите операцию: 0 - Сложение, 1 - Умножение");
        int operation = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите длину матрицы:");
        int length = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите ширину матрицы:");
        int width = int.Parse(Console.ReadLine());

        Console.WriteLine("Введите элементы матрицы через пробел:");
        var matrixData = Console.ReadLine().Split().Select(int.Parse).ToList();

        var request = new MatrixRequest
        {
            Length = length,
            Width = width,
            Operation = (OperationType)operation,
            MatrixData = { matrixData }
        };

        var response = await client.PerformMatrixOperationAsync(request);

        // Console.WriteLine("Результат:");
        // foreach (var item in response.Result)
        // {
        //     Console.Write(item + " ");
        // }
        // Console.WriteLine();
        Console.WriteLine("Результат операции:");
        Console.WriteLine(string.Join(" ", response.Result));
    }

   
    
}

