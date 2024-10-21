namespace Integration.Orchestrator.Backend.Domain.Helper
{
    public static class ConfigurationSystem
    {
        public static string DateTimeFormat { get; set; } = "yyyy-MM-ddTHH:mm:ss.fffZ";
        //public static string DateTimeDefault { get; set; } = GetTimeDefault();

        public static string DateTimeDefault()
        {
            try
            {
                return DateTime.Now.ToString(ConfigurationSystem.DateTimeFormat);
            }
            catch (TimeZoneNotFoundException)
            {
                return DateTime.Now.ToString(ConfigurationSystem.DateTimeFormat); 
            }
            catch (InvalidTimeZoneException)
            {
                return DateTime.Now.ToString(ConfigurationSystem.DateTimeFormat); 
            }
        }

    }
}
    