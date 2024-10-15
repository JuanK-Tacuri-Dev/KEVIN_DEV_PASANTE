namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador
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
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;
    }
}
