using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using System.Text.Json.Serialization;

namespace Integration.Orchestrator.Backend.Domain.Dto.Configurador
{
    public class ServerDto : ServerEntity
    {

        public string TypeServerName { get; set; }
        public List<CatalogEntity> catalogo { get; set; }

    }

}
