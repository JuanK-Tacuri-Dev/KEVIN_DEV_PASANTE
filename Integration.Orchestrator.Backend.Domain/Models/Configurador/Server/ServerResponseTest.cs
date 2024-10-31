using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Helper;
using System.Text.Json.Serialization;

namespace Integration.Orchestrator.Backend.Domain.Models.Configurador.Server
{
    public class ServerResponseTest
    {
        [JsonPropertyName("Id")]
        public Guid id { get; set; }

        [JsonPropertyName("StatusId")]
        public Guid status_id { get; set; }

        [JsonPropertyName("Code")]
        public string server_code { get; set; }

        [JsonPropertyName("Name")]
        public string server_name { get; set; }

        [JsonPropertyName("TypeServerId")]
        public Guid? type_id { get; set; }

        [JsonPropertyName("Url")]
        public string server_url { get; set; }

        [JsonPropertyName("TypeServerName")]
        public string TypeServerName { get; set; }
        [JsonIgnore]
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        [JsonIgnore]
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault();

        [JsonIgnore]
        public List<CatalogEntity> catalogo { get; set; }


    }
}
