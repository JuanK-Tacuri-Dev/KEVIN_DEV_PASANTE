namespace Integration.Orchestrator.Backend.Domain.Commons
{
    public class ResponseMessage
    {
        public ResponseCode Key { get; set; }
        public string Value { get; set; }


    }

    public static class ResponseMessageValues
    {
        private static readonly List<ResponseMessage> responseMessages = new List<ResponseMessage>
        {
            new ResponseMessage { Key = ResponseCode.CreatedSuccessfully, Value = "Creado Correctamente" },
            new ResponseMessage { Key = ResponseCode.NotCreatedSuccessfully, Value = "No Creado Correctamente" },
            new ResponseMessage { Key = ResponseCode.UpdatedSuccessfully, Value = "Actualizado Correctamente" },
            new ResponseMessage { Key = ResponseCode.NotUpdatedSuccessfully, Value = "No Actualizado Correctamente" },
            new ResponseMessage { Key = ResponseCode.FoundSuccessfully, Value = "Encontrado Correctamente" },
            new ResponseMessage { Key = ResponseCode.NotFoundSuccessfully, Value = "No Encontrado Correctamente" },
            new ResponseMessage { Key = ResponseCode.DeletedSuccessfully, Value = "Eliminado Correctamente" },
            new ResponseMessage { Key = ResponseCode.NotDeletedSuccessfully, Value = "No Eliminado Correctamente" }
        };

        // Método que devuelve el mensaje correspondiente a un código
        public static string GetResponseMessage(ResponseCode code)
        {
            var message = responseMessages.FirstOrDefault(m => m.Key == code);
            return message != null ? message.Value : "Código no encontrado";
        }
    }
    
}
