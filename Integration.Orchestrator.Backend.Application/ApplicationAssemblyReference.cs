using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Integration.Orchestrator.Backend.Application
{
    [ExcludeFromCodeCoverage]
    public class ApplicationAssemblyReference
    {
        internal readonly Assembly Assembly = typeof(ApplicationAssemblyReference).Assembly;
    }
}
