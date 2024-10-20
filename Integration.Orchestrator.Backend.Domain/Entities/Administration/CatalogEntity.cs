using Integration.Orchestrator.Backend.Domain.Helper;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class CatalogEntity : Entity<Guid>
    {
        public int catalog_code { get; set; }
        public string catalog_name { get; set; }
        public string catalog_value { get; set; }
        public string catalog_detail { get; set; }
        private int? _father_code;
        public int? father_code
        {
            get => _father_code;
            set
            {
                _father_code = value;
                is_father = _father_code == null;
            }
        }
        public bool is_father { get; set; } = false;
        public Guid status_id { get; set; }
        public string created_at { get; set; } = ConfigurationSystem.DateTimeDefault;
        public string updated_at { get; set; } = ConfigurationSystem.DateTimeDefault;
    }
}
