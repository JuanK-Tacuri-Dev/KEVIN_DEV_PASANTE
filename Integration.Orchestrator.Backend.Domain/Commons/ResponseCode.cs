namespace Integration.Orchestrator.Backend.Domain.Commons
{
    public enum ResponseCode
    {
        CreatedSuccessfully = 10,
        NotCreatedSuccessfully = 11,
        UpdatedSuccessfully = 30,
        NotUpdatedSuccessfully = 31,
        FoundSuccessfully = 20,
        NotFoundSuccessfully = 21,
        DeletedSuccessfully = 40,
        NotDeletedSuccessfully = 42,
        NotValidationSuccessfully = 50 
    }
}
