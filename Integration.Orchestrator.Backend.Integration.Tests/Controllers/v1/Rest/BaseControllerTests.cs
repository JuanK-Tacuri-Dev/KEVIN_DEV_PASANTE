using Integration.Orchestrator.Backend.Application.Models;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Integration.Tests.Factory;
using System.Net.Http.Json;
using System.Text.Json;

namespace Integration.Orchestrator.Backend.Integration.Tests.Controllers.v1.Rest
{
    public abstract class BaseControllerTests
    {
        protected readonly HttpClient Client;
        protected readonly JsonSerializerOptions JsonOptions;
        protected readonly string BaseUrl;

        protected BaseControllerTests(CustomWebApplicationFactoryFixture fixture, string baseUrl)
        {
            Client = fixture?.GetHttpClient() ?? throw new ArgumentNullException(nameof(fixture));
            BaseUrl = baseUrl;
            JsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true,
            };
        }

        protected async Task<TResponse> GetResponseAsync<TResponse>(string relativeUrl)
        {
            var requestUrl = new Uri($"{BaseUrl}/{relativeUrl}", UriKind.Relative);
            var response = await Client.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
            {
                throw new InvalidOperationException("Response content is null.");
            }

            var result = JsonSerializer.Deserialize<TResponse>(content, JsonOptions);

            return result == null ? throw new InvalidOperationException("Deserialization returned null.") : result;
        }

        protected async Task<T> PostResponseAsync<T>(string relativeUrl, object request)
        {
            var requestUrl = new Uri($"{BaseUrl}/{relativeUrl}", UriKind.Relative);
            var response = await Client.PostAsJsonAsync(requestUrl, request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
            {
                throw new InvalidOperationException("Response content is null.");
            }

            var result = JsonSerializer.Deserialize<T>(content, JsonOptions);

            return result == null ? throw new InvalidOperationException("Deserialization returned null.") : result;
        }

        protected static void AssertResponse<T>(ModelResponse<T>? response, ResponseCode expectedStatusCode, string? expectedDescription) where T : class
        {

            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal((int)expectedStatusCode, response.Code);
            Assert.Equal([expectedDescription], response.Messages);
        }

        protected static void AssertResponse<T>(ModelResponseGetAll<T>? response, ResponseCode expectedStatusCode, string? expectedDescription) where T : class
        {
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.Equal((int)expectedStatusCode, response.Code);
            Assert.Equal(expectedDescription, response.Description);
        }

        protected static void AssertCollectionResponse<T>(ModelResponse<IEnumerable<T>> response) where T : class
        {
            Assert.NotNull(response);
            Assert.NotNull(response.Data);
            Assert.True(response.Data.Any());
        }

        protected static (int totalPages, int lastPageRecords) CalculatePagesAndLastPageRecords(int totalRecords, int recordsPerPage)
        {
            // Calcular el total de páginas redondeando hacia arriba
            int totalPages = (int)Math.Ceiling((double)totalRecords / recordsPerPage);

            // Calcular cuántos registros tiene la última página
            int lastPageRecords = totalRecords % recordsPerPage;

            // Si el residuo es 0, significa que la última página está completa
            if (lastPageRecords == 0 && totalRecords > 0)
            {
                lastPageRecords = recordsPerPage;
            }

            return (totalPages, lastPageRecords);
        }

    }
}
