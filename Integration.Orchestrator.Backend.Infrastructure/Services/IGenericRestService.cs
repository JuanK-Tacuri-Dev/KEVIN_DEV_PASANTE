namespace Integration.Orchestrator.Backend.Infrastructure.Services
{
    public interface IGenericRestService
    {
        /// <summary>
        /// Realizar peticion POST
        /// </summary>
        /// <typeparam key="T">Tipo Modelo</typeparam>
        /// <param key="url">Url del endPoint</param>
        /// <param key="body">Datos de la peticion</param>
        /// <param key="camelCaseTransform">(True) para ingnorar camelCase </param>
        /// <param key="skipCert">(True) si quieres omitir certificado SSL invalido. Usar esta opcion solo en caso de emergencia.</param>
        /// <param key="header">(True) si se requiere header para enviar información del cliente</param>
        /// <returns></returns>
        Task<T> PostAsync<T>(string url, object body, bool camelCaseTransform = false, bool skipCert = false, Dictionary<string, string>? header = null);
        /// <summary>
        /// Realizar peticion PUT
        /// </summary>
        /// <typeparam key="T">Tipo Modelo</typeparam>
        /// <param key="url">Url del endPoint</param>
        /// <param key="body">Datos de la peticion</param>        
        /// <param key="skipCert">(True) si quieres omitir certificado SSL invalido. Usar esta opcion solo en caso de emergencia.</param>
        /// <param key="header">(True) si se requiere header para enviar información del cliente</param>
        /// <returns></returns>
        Task<T> PutAsync<T>(string url, object body, bool skipCert = false, Dictionary<string, string>? header = null);
        /// <summary>
        /// Realizar peticion GET
        /// </summary>
        /// <typeparam key="T">Tipo Modelo</typeparam>
        /// <param key="url">Url del endPoint</param>                
        /// <param key="skipCert">(True) si quieres omitir certificado SSL invalido. Usar esta opcion solo en caso de emergencia.</param>
        /// <param key="header">(True) si se requiere header para enviar información del cliente</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string url, bool skipCert = false, Dictionary<string, string>? header = null, Dictionary<string, string>? queryParams = null);
    }
}
