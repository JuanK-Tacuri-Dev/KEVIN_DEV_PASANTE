using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Options
{
    [ExcludeFromCodeCoverage]
    public class LegacyOptions
    {
        public static readonly string Section = "LegacyDB";
        public string ConnectionString { get; set; }
    }
}
