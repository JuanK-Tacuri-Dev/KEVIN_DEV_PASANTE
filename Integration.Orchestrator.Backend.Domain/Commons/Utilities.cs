namespace Integration.Orchestrator.Backend.Domain.Commons
{
    public static class Utilities
    {
        public static string GetSafeKey(string propertyName, int index)
        {
            var parts = propertyName.Split('_');
            return parts.Length > index ? parts[index] : propertyName;
        }
    }
}
