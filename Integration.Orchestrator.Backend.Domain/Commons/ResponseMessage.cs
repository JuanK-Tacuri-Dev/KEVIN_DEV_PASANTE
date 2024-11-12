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

        public static string GetResponseMessage(ResponseCode code, params object[] descMessage)
        {
            var message = code switch
            {
                ResponseCode.CreatedSuccessfully => AppMessages.Domain_ResponseCode_CreatedSuccessfully,
                ResponseCode.NotCreatedSuccessfully => AppMessages.Domain_ResponseCode_NotCreatedSuccessfully,
                ResponseCode.UpdatedSuccessfully => AppMessages.Domain_ResponseCode_UpdatedSuccessfully,
                ResponseCode.NotUpdatedSuccessfully => AppMessages.Domain_ResponseCode_NotUpdatedSuccessfully,
                ResponseCode.FoundSuccessfully => AppMessages.Domain_ResponseCode_FoundSuccessfully,
                ResponseCode.NotFoundSuccessfully => AppMessages.Domain_ResponseCode_NotFoundSuccessfully,
                ResponseCode.DeletedSuccessfully => AppMessages.Domain_ResponseCode_DeletedSuccessfully,
                ResponseCode.NotDeletedSuccessfully => AppMessages.Domain_ResponseCode_NotDeletedSuccessfully,
                ResponseCode.NotValidationSuccessfully => AppMessages.Domain_ResponseCode_NotValidationSuccessfully,
                ResponseCode.NotDeleteDueToRelationship => AppMessages.Domain_ResponseCode_NotDeleteDueToRelationship,
                ResponseCode.NotActivatedDueToInactiveRelationship => AppMessages.Domain_ResponseCode_NotActivatedDueToInactiveRelationship,
                _ => AppMessages.Domain_ResponseCode_NotFoundCodeSuccessfully
            };

            return descMessage != null && descMessage.Length > 0 ? string.Format(message, descMessage) : message;
        }
    }

}
