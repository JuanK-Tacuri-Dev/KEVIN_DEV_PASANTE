namespace Integration.Orchestrator.Backend.Domain.Models.Configurador.Catalog
{
    public class CatalogResponseModel
    {
        public Guid Id { get; set; }
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
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public List<StatusResponseModel> Status { get; set; }
    }

    public class StatusResponseModel  {
        public string status_key { get; set; }
        public string status_text { get; set; }
        public string status_color { get; set; }
        public string status_background { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public Guid Id { get; set; }
    }
}
