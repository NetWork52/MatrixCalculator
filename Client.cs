// ПРОВЕРИТЬ!!!!!!!! (ИСПРАВИТЬ)

using Grpc.Net.Client;
using Matrix;  // Пространство имен из сгенерированных классов (MatrixRequest, MatrixResponse)

public class Client
{
    private readonly string _serverAddress;

    public Client(string serverAddress)
    {
        _serverAddress = serverAddress;
    }

    public async Task<MatrixResponse> PerformMatrixOperationAsync(MatrixRequest request)
    {
        // Создаем канал для подключения к gRPC серверу
        var channel = GrpcChannel.ForAddress(_serverAddress);

        // Создаем клиент для работы с MatrixService
        var client = new MatrixService.MatrixServiceClient(channel);

        // Отправляем запрос и получаем результат
        var response = await client.PerformMatrixOperationAsync(request);

        return response;
    }
}
