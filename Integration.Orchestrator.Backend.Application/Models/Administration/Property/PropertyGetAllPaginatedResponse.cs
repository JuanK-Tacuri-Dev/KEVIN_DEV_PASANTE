namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class PropertyGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<PropertyGetAllPaginated>>
    {
    }
    public class PropertyGetAllPaginated : PropertyRequest
    {
        public Guid Id { get; set; }
    }
}
