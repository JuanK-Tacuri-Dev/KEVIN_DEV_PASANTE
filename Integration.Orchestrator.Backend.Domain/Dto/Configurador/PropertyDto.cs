using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Orchestrator.Backend.Domain.Dto.Configurador
{
    internal class PropertyDto
    {
        public string TypeEntityName { get; set; }
        public List<CatalogEntity> catalogo { get; set; }
    }
}
