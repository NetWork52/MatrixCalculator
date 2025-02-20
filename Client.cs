
using System.IO;
using Grpc.Net.Client;
using Matrix;  // Пространство имен из сгенерированных классов (MatrixRequest, MatrixResponse)
using System;
using System.Linq;
using System.Threading.Tasks;

public class FileInputClient
{
    private readonly string _serverAddress;
    private readonly string _filePath;

    public FileInputClient(string serverAddress, string filePath)
    {
        _serverAddress = serverAddress;
        _filePath = filePath;
    }

    public async Task Run()
    {
        // Чтение данных из файла
        var lines = File.ReadAllLines(_filePath);
        
        // Операция (0 - сложение, 1 - умножение)
        int operation = int.Parse(lines[0]);

        // Длина и ширина матрицы
        int length = int.Parse(lines[1]);
        int width = int.Parse(lines[2]);

        // Элементы матриц (матрица может быть только одна для операции сложения или две для умножения)
        var matrixData = lines[3].Split().Select(int.Parse).ToList();

        // Если операция умножение, то считаем, что в файле есть вторая матрица
        var secondMatrixData = operation == 1 ? lines[4].Split().Select(int.Parse).ToList() : null;

        var request = new MatrixRequest
        {
            Length = length,
            Width = width,
            Operation = (OperationType)operation,
            MatrixData = { matrixData }
        };

        // Добавляем вторую матрицу в запрос для операции умножения
        if (secondMatrixData != null)
        {
            request.MatrixData.AddRange(secondMatrixData);
        }

        // Отправка запроса на сервер
        using var channel = GrpcChannel.ForAddress(_serverAddress);
        var client = new MatrixService.MatrixServiceClient(channel);
        
        var response = await client.PerformMatrixOperationAsync(request);

        Console.WriteLine("Результат операции:");
        Console.WriteLine(string.Join(" ", response.Result));
    }
}



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

