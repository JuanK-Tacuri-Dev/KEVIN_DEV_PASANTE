
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Integration.Orchestrator.Backend.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    public class GenericRestService : IGenericRestService
    {
        private readonly HttpClient _httpClient;

        public GenericRestService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<T> PostAsync<T>(string url, object body, bool camelCaseTransform = false, bool skipCert = false, Dictionary<string, string> header = null)
        {
            if (skipCert)
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            AddHeaders(header);

            HttpResponseMessage response = await _httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(body, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() }), Encoding.UTF8, "application/json"));

            return await HandleResponse<T>(response);
        }

        public async Task<T> PutAsync<T>(string url, object body, bool skipCert = false, Dictionary<string, string> header = null)
        {
            if (skipCert)
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            AddHeaders(header);

            HttpResponseMessage response = await _httpClient.PutAsync(url, new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json"));

            return await HandleResponse<T>(response);
        }

        public async Task<T> GetAsync<T>(string url, bool skipCert = false, Dictionary<string, string> header = null, Dictionary<string, string> queryParams = null)
        {
            if (skipCert)
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            }

            AddHeaders(header);

            if (queryParams != null && queryParams.Count > 0)
            {
                url += "?" + string.Join("&", queryParams.Select(x => $"{x.Key}={x.Value}"));
            }

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            return await HandleResponse<T>(response);
        }

        private void AddHeaders(Dictionary<string, string> header)
        {
            if (header != null && header.Count > 0)
            {
                foreach (var keyValuePair in header)
                {
                    _httpClient.DefaultRequestHeaders.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseData);
            }
            else
            {
                throw new HttpRequestException($"HTTP request failed with status code {response.StatusCode}");
            }
        }
    }
}
