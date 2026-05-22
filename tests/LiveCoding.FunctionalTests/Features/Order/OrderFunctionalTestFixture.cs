using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace LiveCoding.FunctionalTests.Features.Order;

public class OrderFunctionalTestFixture : IAsyncLifetime
{
    private readonly CustomWebApplicationFactory _factory;
    public HttpClient Client { get; }
    public static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
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
