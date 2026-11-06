using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace backend.Tests;

public class HelloEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public HelloEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetHello_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/hello");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetHello_ReturnsJsonContentType()
    {
        var response = await _client.GetAsync("/api/hello");

        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }

    [Fact]
    public async Task GetHello_ReturnsExpectedMessage()
    {
        var json = await _client.GetFromJsonAsync<JsonElement>("/api/hello");

        Assert.Equal("Hello from GH200 backend!", json.GetProperty("message").GetString());
    }

    [Fact]
    public async Task GetHello_ReturnsCurrentUtcTimestamp()
    {
        var before = DateTimeOffset.UtcNow.AddSeconds(-5);

        var json = await _client.GetFromJsonAsync<JsonElement>("/api/hello");

        var timestamp = json.GetProperty("timestamp").GetDateTimeOffset();
        Assert.InRange(timestamp, before, DateTimeOffset.UtcNow.AddSeconds(5));
    }

    [Fact]
    public async Task GetHello_ResponseContainsOnlyMessageAndTimestamp()
    {
        var json = await _client.GetFromJsonAsync<JsonElement>("/api/hello");

        var properties = json.EnumerateObject().Select(p => p.Name).OrderBy(n => n).ToArray();
        Assert.Equal(new[] { "message", "timestamp" }, properties);
    }
}
