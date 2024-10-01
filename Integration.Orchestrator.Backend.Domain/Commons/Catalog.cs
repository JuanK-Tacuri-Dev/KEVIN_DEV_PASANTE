namespace Integration.Orchestrator.Backend.Domain.Commons
{
    public enum Prefix
    {
        Extract = 8,
        Transform = 9,
        Load = 10,
        Server = 11,
        Adapter = 12,
        Repository = 13,
        Connection = 14,
        Process = 15,
        Entity = 16,
        Property = 17,
        Catalog = 18,
        Integrations = 19,
        Synchronyzation = 20
    }
    public enum ParentCatalog
    {
        Prefix = 1,
        ServerType = 2,
        AdapterType = 3,
        AuthenticationType = 4,
        ProcessType = 5,
        DataType = 6,
        OperatorType = 7
    }
}
