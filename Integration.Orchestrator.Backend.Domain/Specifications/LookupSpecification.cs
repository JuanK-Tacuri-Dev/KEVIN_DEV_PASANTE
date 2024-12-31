namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class LookupSpecification<T>
    {
        public string Collection { get; set; } = string.Empty;
        public string LocalField { get; set; }
        public string ForeignField { get; set; }
        public string As { get; set; }
    }
}
