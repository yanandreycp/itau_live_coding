using System.Text.Json;

namespace LiveCoding.FunctionalTests;

public class OrderFunctionalTestFixture : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    public HttpClient Client { get; }
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public OrderFunctionalTestFixture()
    {
        _factory = new CustomWebApplicationFactory();
        Client = _factory.CreateClient();
    }

    public Task InitializeAsync()
    {
        _factory.InitializeDatabase();
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        _factory.Dispose();
        return Task.CompletedTask;
    }
}
