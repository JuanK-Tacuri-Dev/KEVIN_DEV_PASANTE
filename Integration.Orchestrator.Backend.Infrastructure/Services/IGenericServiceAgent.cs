namespace Integration.Orchestrator.Backend.Infrastructure.Services
{
    public interface IGenericServiceAgent
    {
        /// <summary>
        /// Realizar peticion POST
        /// </summary>
        /// <typeparam name="T">Tipo Modelo</typeparam>
        /// <param name="url">Url del endPoint</param>
        /// <param name="body">Datos de la peticion</param>
        /// <param name="camelCaseTransform">(True) para ingnorar camelCase </param>
        /// <param name="skipCert">(True) si quieres omitir certificado SSL invalido. Usar esta opcion solo en caso de emergencia.</param>
        /// <param name="header">(True) si se requiere header para enviar información del cliente</param>
        /// <returns></returns>
        Task<T> PostAsync<T>(string url, object body, bool camelCaseTransform = false, bool skipCert = false, Dictionary<string, string> header = null);
        /// <summary>
        /// Realizar peticion PUT
        /// </summary>
        /// <typeparam name="T">Tipo Modelo</typeparam>
        /// <param name="url">Url del endPoint</param>
        /// <param name="body">Datos de la peticion</param>        
        /// <param name="skipCert">(True) si quieres omitir certificado SSL invalido. Usar esta opcion solo en caso de emergencia.</param>
        /// <param name="header">(True) si se requiere header para enviar información del cliente</param>
        /// <returns></returns>
        Task<T> PutAsync<T>(string url, object body, bool skipCert = false, Dictionary<string, string> header = null);
        /// <summary>
        /// Realizar peticion GET
        /// </summary>
        /// <typeparam name="T">Tipo Modelo</typeparam>
        /// <param name="url">Url del endPoint</param>                
        /// <param name="skipCert">(True) si quieres omitir certificado SSL invalido. Usar esta opcion solo en caso de emergencia.</param>
        /// <param name="header">(True) si se requiere header para enviar información del cliente</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string url, bool skipCert = false, Dictionary<string, string> header = null, Dictionary<string, string> queryParams = null);
    }
}
