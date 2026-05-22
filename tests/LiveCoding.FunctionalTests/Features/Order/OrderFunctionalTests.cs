using LiveCoding.Application.UseCases.ChangeProductQuantity;
using LiveCoding.Application.UseCases.CreateOrder;
using LiveCoding.Application.UseCases.GetOrder;
using LiveCoding.Application.UseCases.RemoveOrderProduct;
using LiveCoding.Domain.Enums;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace LiveCoding.FunctionalTests.Features.Order;

public class OrderFunctionalTests : IClassFixture<OrderFunctionalTestFixture>
{
    private readonly HttpClient _client;

    public OrderFunctionalTests(OrderFunctionalTestFixture fixture)
    {
        _client = fixture.Client;
    }

    [Fact]
    public async Task CreateOrder_WithStandardType_ReturnsCreatedOrderWithCorrectPrices()
    {
        var input = new CreateOrderInput
        {
            Type = EOrderType.Standard,
            Products =
            [
                new CreateOrderProductInput { Name = "Produto A", Quantity = 2, Price = 100m }
            ]
        };

        var response = await _client.PostAsJsonAsync("orders", input, OrderFunctionalTestFixture.JsonOptions);
        var output = await response.Content.ReadFromJsonAsync<CreateOrderOutput>(OrderFunctionalTestFixture.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(output);
        Assert.NotEqual(Guid.Empty, output.Id);
        Assert.Single(output.Products);
        Assert.Equal(EOrderType.Standard, output.Type);
        Assert.Equal("Produto A", output.Products[0].Name);
        Assert.Equal(2, output.Products[0].Quantity);
        Assert.Equal(100m, output.Products[0].OriginalUnitPrice);
        Assert.Equal(100m, output.Products[0].EffectiveUnitPrice);
        Assert.Equal(0m, output.Products[0].Rate);
        Assert.Equal(0m, output.Products[0].Delta);
        Assert.Equal(200m, output.Products[0].TotalPrice);
        Assert.Equal(200m, output.InitialPrice);
        Assert.Equal(200m, output.EffectivePrice);
    }

    [Fact]
    public async Task CreateOrder_WithExpressType_Applies15PercentSurcharge()
    {
        var input = new CreateOrderInput
        {
            Type = EOrderType.Express,
            Products =
            [
                new CreateOrderProductInput { Name = "Produto Express", Quantity = 3, Price = 50m }
            ]
        };

        var response = await _client.PostAsJsonAsync("orders", input, OrderFunctionalTestFixture.JsonOptions);
        var output = await response.Content.ReadFromJsonAsync<CreateOrderOutput>(OrderFunctionalTestFixture.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(output);
        Assert.Equal(EOrderType.Express, output.Type);
        Assert.Equal(0.15m, output.Products[0].Rate);
        Assert.Equal(50m, output.Products[0].OriginalUnitPrice);
        Assert.Equal(57.5m, output.Products[0].EffectiveUnitPrice);
        Assert.Equal(22.5m, output.Products[0].Delta);
        Assert.Equal(172.5m, output.Products[0].TotalPrice);
        Assert.Equal(150m, output.InitialPrice);
        Assert.Equal(172.5m, output.EffectivePrice);
    }

    [Fact]
    public async Task CreateOrder_WithSubscriptionType_Applies10PercentDiscount()
    {
        var input = new CreateOrderInput
        {
            Type = EOrderType.Subscription,
            Products =
            [
                new CreateOrderProductInput { Name = "Produto Sub", Quantity = 5, Price = 80m }
            ]
        };

        var response = await _client.PostAsJsonAsync("orders", input, OrderFunctionalTestFixture.JsonOptions);
        var output = await response.Content.ReadFromJsonAsync<CreateOrderOutput>(OrderFunctionalTestFixture.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(output);
        Assert.Equal(EOrderType.Subscription, output.Type);
        Assert.Equal(-0.10m, output.Products[0].Rate);
        Assert.Equal(80m, output.Products[0].OriginalUnitPrice);
        Assert.Equal(72m, output.Products[0].EffectiveUnitPrice);
        Assert.Equal(-40m, output.Products[0].Delta);
        Assert.Equal(360m, output.Products[0].TotalPrice);
        Assert.Equal(400m, output.InitialPrice);
        Assert.Equal(360m, output.EffectivePrice);
    }

    [Fact]
    public async Task CreateOrder_WithMultipleProducts_CalculatesCorrectTotals()
    {
        var input = new CreateOrderInput
        {
            Type = EOrderType.Express,
            Products =
            [
                new CreateOrderProductInput { Name = "P1", Quantity = 2, Price = 100m },
                new CreateOrderProductInput { Name = "P2", Quantity = 1, Price = 200m },
                new CreateOrderProductInput { Name = "P3", Quantity = 4, Price = 50m }
            ]
        };

        var response = await _client.PostAsJsonAsync("orders", input, OrderFunctionalTestFixture.JsonOptions);
        var output = await response.Content.ReadFromJsonAsync<CreateOrderOutput>(OrderFunctionalTestFixture.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(output);
        Assert.Equal(3, output.Products.Count);
        Assert.Equal(200m, output.Products[0].OriginalUnitPrice * 2);
        Assert.Equal(0.15m, output.Products[0].Rate);
        Assert.Equal(600m, output.InitialPrice);
        Assert.Equal(690m, output.EffectivePrice);
    }

    [Fact]
    public async Task CreateOrder_WithEmptyProducts_ReturnsBadRequest()
    {
        var input = new CreateOrderInput
        {
            Type = EOrderType.Standard,
            Products = []
        };

        var response = await _client.PostAsJsonAsync("orders", input, OrderFunctionalTestFixture.JsonOptions);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetOrder_WithExistingId_ReturnsOrderWithDetails()
    {
        var createInput = new CreateOrderInput
        {
            Type = EOrderType.Standard,
            Products =
            [
                new CreateOrderProductInput { Name = "Produto GET", Quantity = 1, Price = 150m }
            ]
        };
        var createResponse = await _client.PostAsJsonAsync("orders", createInput, OrderFunctionalTestFixture.JsonOptions);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateOrderOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(created);

        var getResponse = await _client.GetAsync($"orders/{created.Id}");
        var getOutput = await getResponse.Content.ReadFromJsonAsync<GetOrderOutput>(OrderFunctionalTestFixture.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.NotNull(getOutput);
        Assert.Equal(created.Id, getOutput.Id);
        Assert.Equal(EOrderType.Standard, getOutput.Type);
        Assert.Single(getOutput.Products);
        Assert.Equal("Produto GET", getOutput.Products[0].Name);
        Assert.Equal(150m, getOutput.InitialPrice);
        Assert.Equal(150m, getOutput.EffectivePrice);
        Assert.NotEqual(default, getOutput.CreatedAt);
    }

    [Fact]
    public async Task GetOrder_WithNonExistentId_ReturnsNoContent()
    {
        var response = await _client.GetAsync($"orders/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task ChangeProductQuantity_OnExistingProduct_UpdatesQuantityAndRecalculates()
    {
        var createInput = new CreateOrderInput
        {
            Type = EOrderType.Express,
            Products =
            [
                new CreateOrderProductInput { Name = "Qty Test", Quantity = 2, Price = 100m }
            ]
        };
        var createResponse = await _client.PostAsJsonAsync("orders", createInput, OrderFunctionalTestFixture.JsonOptions);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateOrderOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(created);

        var productId = created.Products[0].Id;
        var changeInput = new { quantity = 5 };

        var putResponse = await _client.PutAsJsonAsync(
            $"orders/{created.Id}/items/{productId}", changeInput, OrderFunctionalTestFixture.JsonOptions);
        var changed = await putResponse.Content.ReadFromJsonAsync<ChangeProductQuantityOutput>(OrderFunctionalTestFixture.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
        Assert.NotNull(changed);
        Assert.Equal(5, changed.Products[0].Quantity);
        Assert.Equal(100m, changed.Products[0].OriginalUnitPrice);
        Assert.Equal(115m, changed.Products[0].EffectiveUnitPrice);
        Assert.Equal(0.15m, changed.Products[0].Rate);
        Assert.Equal(575m, changed.Products[0].TotalPrice);
        Assert.Equal(500m, changed.InitialPrice);
        Assert.Equal(575m, changed.EffectivePrice);
        Assert.NotNull(changed.UpdatedAt);
    }

    [Fact]
    public async Task ChangeProductQuantity_WithInvalidQuantity_ReturnsBadRequest()
    {
        var createInput = new CreateOrderInput
        {
            Type = EOrderType.Standard,
            Products =
            [
                new CreateOrderProductInput { Name = "Inv Qty", Quantity = 1, Price = 10m }
            ]
        };
        var createResponse = await _client.PostAsJsonAsync("orders", createInput, OrderFunctionalTestFixture.JsonOptions);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateOrderOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(created);

        var changeInput = new { quantity = 0 };

        var putResponse = await _client.PutAsJsonAsync(
            $"orders/{created.Id}/items/{created.Products[0].Id}", changeInput, OrderFunctionalTestFixture.JsonOptions);

        Assert.Equal(HttpStatusCode.BadRequest, putResponse.StatusCode);
    }

    [Fact]
    public async Task RemoveOrderProduct_OnExistingProduct_RemovesAndRecalculates()
    {
        var createInput = new CreateOrderInput
        {
            Type = EOrderType.Standard,
            Products =
            [
                new CreateOrderProductInput { Name = "Removível", Quantity = 2, Price = 100m },
                new CreateOrderProductInput { Name = "Permanente", Quantity = 3, Price = 50m }
            ]
        };
        var createResponse = await _client.PostAsJsonAsync("orders", createInput, OrderFunctionalTestFixture.JsonOptions);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateOrderOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(created);
        Assert.Equal(2, created.Products.Count);

        var deleteResponse = await _client.DeleteAsync($"orders/{created.Id}/items/{created.Products[0].Id}");
        var result = await deleteResponse.Content.ReadFromJsonAsync<RemoveOrderProductOutput>(OrderFunctionalTestFixture.JsonOptions);

        Assert.Equal(HttpStatusCode.OK, deleteResponse.StatusCode);
        Assert.NotNull(result);
        Assert.Single(result.Products);
        Assert.Equal("Permanente", result.Products[0].Name);
        Assert.Equal(150m, result.InitialPrice);
        Assert.Equal(150m, result.EffectivePrice);
    }

    [Fact]
    public async Task RemoveOrderProduct_WithNonExistentOrder_ReturnsNoContent()
    {
        var deleteResponse = await _client.DeleteAsync($"orders/{Guid.NewGuid()}/items/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
    }

    [Fact]
    public async Task FullFlow_CreateStandardOrder_ChangeQuantity_RemoveProduct_ValidateConsistency()
    {
        var createInput = new CreateOrderInput
        {
            Type = EOrderType.Standard,
            Products =
            [
                new CreateOrderProductInput { Name = "Item A", Quantity = 2, Price = 100m },
                new CreateOrderProductInput { Name = "Item B", Quantity = 3, Price = 50m }
            ]
        };
        var createResponse = await _client.PostAsJsonAsync("orders", createInput, OrderFunctionalTestFixture.JsonOptions);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateOrderOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(created);
        Assert.Equal(350m, created.InitialPrice);
        Assert.Equal(350m, created.EffectivePrice);

        var itemA = created.Products[0];

        var changeInput = new { quantity = 5 };
        var putResponse = await _client.PutAsJsonAsync(
            $"orders/{created.Id}/items/{itemA.Id}", changeInput, OrderFunctionalTestFixture.JsonOptions);
        var changed = await putResponse.Content.ReadFromJsonAsync<ChangeProductQuantityOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(changed);
        Assert.Equal(5, changed.Products[0].Quantity);
        Assert.Equal(650m, changed.InitialPrice);
        Assert.Equal(650m, changed.EffectivePrice);

        var deleteResponse = await _client.DeleteAsync($"orders/{created.Id}/items/{itemA.Id}");
        var removed = await deleteResponse.Content.ReadFromJsonAsync<RemoveOrderProductOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(removed);
        Assert.Single(removed.Products);
        Assert.Equal("Item B", removed.Products[0].Name);
        Assert.Equal(150m, removed.InitialPrice);
        Assert.Equal(150m, removed.EffectivePrice);

        var getResponse = await _client.GetAsync($"orders/{created.Id}");
        var getOutput = await getResponse.Content.ReadFromJsonAsync<GetOrderOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(getOutput);
        Assert.Single(getOutput.Products);
        Assert.Equal(150m, getOutput.EffectivePrice);
        Assert.NotNull(getOutput.UpdatedAt);
    }

    [Fact]
    public async Task FullFlow_CreateExpressOrder_ChangeQuantity_ValidatePrices()
    {
        var createInput = new CreateOrderInput
        {
            Type = EOrderType.Express,
            Products =
            [
                new CreateOrderProductInput { Name = "Express Item", Quantity = 2, Price = 200m }
            ]
        };
        var createResponse = await _client.PostAsJsonAsync("orders", createInput, OrderFunctionalTestFixture.JsonOptions);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateOrderOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(created);
        Assert.Equal(200m, created.Products[0].OriginalUnitPrice);
        Assert.Equal(230m, created.Products[0].EffectiveUnitPrice);
        Assert.Equal(0.15m, created.Products[0].Rate);
        Assert.Equal(460m, created.Products[0].TotalPrice);
        Assert.Equal(400m, created.InitialPrice);
        Assert.Equal(460m, created.EffectivePrice);

        var changeInput = new { quantity = 3 };
        var putResponse = await _client.PutAsJsonAsync(
            $"orders/{created.Id}/items/{created.Products[0].Id}", changeInput, OrderFunctionalTestFixture.JsonOptions);
        var changed = await putResponse.Content.ReadFromJsonAsync<ChangeProductQuantityOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(changed);
        Assert.Equal(3, changed.Products[0].Quantity);
        Assert.Equal(0.15m, changed.Products[0].Rate);
        Assert.Equal(690m, changed.Products[0].TotalPrice);
        Assert.Equal(600m, changed.InitialPrice);
        Assert.Equal(690m, changed.EffectivePrice);

        var getResponse = await _client.GetAsync($"orders/{created.Id}");
        var getOutput = await getResponse.Content.ReadFromJsonAsync<GetOrderOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(getOutput);
        Assert.Equal(690m, getOutput.EffectivePrice);
    }

    [Fact]
    public async Task FullFlow_CreateSubscriptionOrder_ChangeQuantity_ValidatePrices()
    {
        var createInput = new CreateOrderInput
        {
            Type = EOrderType.Subscription,
            Products =
            [
                new CreateOrderProductInput { Name = "Sub Item", Quantity = 4, Price = 250m }
            ]
        };
        var createResponse = await _client.PostAsJsonAsync("orders", createInput, OrderFunctionalTestFixture.JsonOptions);
        var created = await createResponse.Content.ReadFromJsonAsync<CreateOrderOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(created);
        Assert.Equal(250m, created.Products[0].OriginalUnitPrice);
        Assert.Equal(225m, created.Products[0].EffectiveUnitPrice);
        Assert.Equal(-0.10m, created.Products[0].Rate);
        Assert.Equal(900m, created.Products[0].TotalPrice);
        Assert.Equal(1000m, created.InitialPrice);
        Assert.Equal(900m, created.EffectivePrice);

        var changeInput = new { quantity = 2 };
        var putResponse = await _client.PutAsJsonAsync(
            $"orders/{created.Id}/items/{created.Products[0].Id}", changeInput, OrderFunctionalTestFixture.JsonOptions);
        var changed = await putResponse.Content.ReadFromJsonAsync<ChangeProductQuantityOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(changed);
        Assert.Equal(2, changed.Products[0].Quantity);
        Assert.Equal(-0.10m, changed.Products[0].Rate);
        Assert.Equal(450m, changed.Products[0].TotalPrice);
        Assert.Equal(500m, changed.InitialPrice);
        Assert.Equal(450m, changed.EffectivePrice);

        var getResponse = await _client.GetAsync($"orders/{created.Id}");
        var getOutput = await getResponse.Content.ReadFromJsonAsync<GetOrderOutput>(OrderFunctionalTestFixture.JsonOptions);
        Assert.NotNull(getOutput);
        Assert.Equal(450m, getOutput.EffectivePrice);
    }
}