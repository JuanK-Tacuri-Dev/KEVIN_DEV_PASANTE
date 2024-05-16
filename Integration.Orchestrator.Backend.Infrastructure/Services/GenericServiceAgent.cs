
namespace Integration.Orchestrator.Backend.Infrastructure.Services
{
    public class GenericServiceAgent : IGenericServiceAgent
    {
        public Task<T> GetAsync<T>(string url, bool skipCert = false, Dictionary<string, string> header = null, Dictionary<string, string> queryParams = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> PostAsync<T>(string url, object body, bool camelCaseTransform = false, bool skipCert = false, Dictionary<string, string> header = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> PutAsync<T>(string url, object body, bool skipCert = false, Dictionary<string, string> header = null)
        {
            throw new NotImplementedException();
        }
    }
}
