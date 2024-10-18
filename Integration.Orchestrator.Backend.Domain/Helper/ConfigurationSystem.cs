namespace Integration.Orchestrator.Backend.Domain.Helper
{
    public static class ConfigurationSystem
    {
        public static string DateTimeFormat { get; set; } = "yyyy-MM-ddTHH:mm:ss.fffZ";
        public static string DateTimeDefault { get; set; } = GetTimeDefault();


        private static string GetTimeDefault()
        {
            try
            {

                #region Las zonas Horarias se las deja comentadas para un posible uso hasta que sea parametrizado desde una fuente de datos.
                /*
                var ZonaHorariaEsteEEUU = "Eastern Standard Time"; // UTC-5
                var ZonaHorariaPacificoEEUU = "Pacific Standard Time"; // UTC-8
                var ZonaHorariaEuropaCentral = "Central European Standard Time"; // UTC+1
                var ZonaHorariaGreenwich = "GMT Standard Time"; // UTC+0
                var ZonaHorariaChina = "China Standard Time"; // UTC+8
                var ZonaHorariaJapon = "Tokyo Standard Time"; // UTC+9
                var ZonaHorariaIndia = "India Standard Time"; // UTC+5:30
                var ZonaHorariaAustraliaEste = "AUS Eastern Standard Time"; // UTC+10
                var ZonaHorariaAlaska = "Alaskan Standard Time"; // UTC-9
                var ZonaHorariaHawai = "Hawaiian Standard Time"; // UTC-10
                var ZonaHorariaArgentina = "Argentina Standard Time"; // UTC-3
                var ZonaHorariaBrasilEste = "E. South America Standard Time"; // UTC-3
                var ZonaHorariaEuropaEste = "E. Europe Standard Time"; // UTC+2
                var ZonaHorariaMoscu = "Russian Standard Time"; // UTC+3
                var ZonaHorariaArabia = "Arabian Standard Time"; // UTC+3
                var ZonaHorariaIran = "Iran Standard Time"; // UTC+3:30
                var ZonaHorariaAfganistan = "Afghanistan Standard Time"; // UTC+4:30
                var ZonaHorariaNuevaZelanda = "New Zealand Standard Time"; // UTC+12
                var ZonaHorariaSamoa = "Samoa Standard Time"; // UTC+13
                */
                #endregion

                var ZonaHorariaPacificoSurAmerica = "SA Pacific Standard Time"; // UTC-5
                var TimeZone = TimeZoneInfo.FindSystemTimeZoneById(ZonaHorariaPacificoSurAmerica);
                var localTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZone);
                return localTime.ToString(ConfigurationSystem.DateTimeFormat);
            }
            catch (TimeZoneNotFoundException)
            {
                return DateTime.UtcNow.ToString(ConfigurationSystem.DateTimeFormat); 
            }
            catch (InvalidTimeZoneException)
            {
                return DateTime.UtcNow.ToString(ConfigurationSystem.DateTimeFormat); 
            }
        }

    }


    

}
    