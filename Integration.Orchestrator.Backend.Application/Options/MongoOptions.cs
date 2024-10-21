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
        public string ConnectionString { get; set; }

        /// <summary>
        ///     Name of database on mongoDB
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        ///     Name of collections
        /// </summary>
        public Collection Collections { get; set; }
    }

    /// <summary>
    ///     This class contains the names of collections
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Collection
    {
        public string Synchronization { get; set; }
        public string SynchronizationStates { get; set; }
        public string Connection { get; set; }
        public string Integration { get; set; }
        public string Process { get; set; }
        public string Status { get; set; }
        public string Entity { get; set; }
        public string Property { get; set; }
        public string Value { get; set; }
        public string Operator { get; set; }
        public string Server { get; set; }
        public string Repository { get; set; }
        public string Adapter { get; set; }
        public string Catalog { get; set; }
        public string CodeConfigurator { get; set; }

    }
}
