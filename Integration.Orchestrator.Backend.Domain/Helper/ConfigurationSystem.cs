using System;

namespace Integration.Orchestrator.Backend.Domain.Helper
{
    public static class ConfigurationSystem
    {
        public static string DateTimeFormat { get; set; } = "yyyy-MM-ddTHH:mm:ss.fffZ";
        public static string RegionZone { get; set; } = "Ecuador Time";

        public static string DateTimeDefault()
        {
            try
            {
                DateTime currentTime = DateTime.Now;
                TimeZoneInfo TimeZone = TimeZoneInfo.FindSystemTimeZoneById(RegionZone);
                DateTime DateRegion = TimeZoneInfo.ConvertTime(currentTime, TimeZoneInfo.Local, TimeZone);

                return DateRegion.ToString(ConfigurationSystem.DateTimeFormat);
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
    