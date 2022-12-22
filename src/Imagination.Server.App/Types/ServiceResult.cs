namespace Imagination.Server.App.Types
{
    public class ServiceResult
    {
        public ServiceErrorType? Error { get; private set; }
        public bool WasSuccessful { get { return Error is null; } }
        public MemoryStream Result { get; private set; }

        public ServiceResult(ServiceErrorType errorType)
        {
            Error = errorType;
        }

        public ServiceResult(MemoryStream stream)
        {
            Result = stream;
        }
    }
}
