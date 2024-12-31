using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Options
{
    /// <summary>
    ///     This class contains the configuration parameters that are mapped from appsettings for mongoDB
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class MongoOptions
    {
        /// <summary>
        ///     Name section to read the configuration in appsettings
        /// </summary>
        public static readonly string Section = "MongoDB";

        /// <summary>
        ///     Connection string for connect to mongoDB
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        ///     Name of database on mongoDB
        /// </summary>
        public string DatabaseName { get; set; } = string.Empty;

        /// <summary>
        ///     Name of collections
        /// </summary>
        public Collection Collections { get; set; } = new Collection();
    }

    /// <summary>
    ///     This class contains the names of collections
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Collection
    {
        public string Synchronization { get; set; } = string.Empty;
        public string SynchronizationStates { get; set; } = string.Empty;
        public string Connection { get; set; } = string.Empty;
        public string Integration { get; set; } = string.Empty;
        public string Process { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Entity { get; set; } = string.Empty;
        public string Property { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string Server { get; set; } = string.Empty;
        public string Repository { get; set; } = string.Empty;
        public string Adapter { get; set; } = string.Empty;
        public string Catalog { get; set; } = string.Empty;
        public string CodeConfigurator { get; set; } = string.Empty;

    }
}
