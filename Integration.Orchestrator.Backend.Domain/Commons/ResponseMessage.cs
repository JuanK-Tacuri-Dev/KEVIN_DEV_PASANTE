using Integration.Orchestrator.Backend.Domain.Resources;

namespace Integration.Orchestrator.Backend.Domain.Commons
{
    public class ResponseMessage
    {
        public ResponseCode Key { get; set; }
        public string Value { get; set; }
    }

    public static class ResponseMessageValues
    {
        public static List<ResponseMessage> ResponseMessages { get; } =
        [
            new ResponseMessage { Key = ResponseCode.CreatedSuccessfully, Value = AppMessages.Domain_ResponseCode_CreatedSuccessfully },
            new ResponseMessage { Key = ResponseCode.NotCreatedSuccessfully, Value = AppMessages.Domain_ResponseCode_NotCreatedSuccessfully },
            new ResponseMessage { Key = ResponseCode.UpdatedSuccessfully, Value = AppMessages.Domain_ResponseCode_UpdatedSuccessfully },
            new ResponseMessage { Key = ResponseCode.NotUpdatedSuccessfully, Value = AppMessages.Domain_ResponseCode_NotUpdatedSuccessfully },
            new ResponseMessage { Key = ResponseCode.FoundSuccessfully, Value = AppMessages.Domain_ResponseCode_FoundSuccessfully },
            new ResponseMessage { Key = ResponseCode.NotFoundSuccessfully, Value = AppMessages.Domain_ResponseCode_NotFoundSuccessfully },
            new ResponseMessage { Key = ResponseCode.DeletedSuccessfully, Value = AppMessages.Domain_ResponseCode_DeletedSuccessfully },
            new ResponseMessage { Key = ResponseCode.NotDeletedSuccessfully, Value = AppMessages.Domain_ResponseCode_NotDeletedSuccessfully },
            new ResponseMessage { Key = ResponseCode.NotValidationSuccessfully, Value = AppMessages.Domain_ResponseCode_NotValidationSuccessfully }
        ];

        // Método que devuelve el mensaje correspondiente a un código
        public static string GetResponseMessage(ResponseCode code)
        {
            var message = ResponseMessages.FirstOrDefault(m => m.Key == code);
            return message != null ? message.Value : AppMessages.Domain_ResponseCode_NotFoundCodeSuccessfully;
        }
    }
    
}
