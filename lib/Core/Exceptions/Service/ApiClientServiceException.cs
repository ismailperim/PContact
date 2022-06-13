namespace Core.Exceptions.Service
{
    internal class ApiClientServiceException : ServiceException
    {
        public ApiClientServiceException() : base("Generic ApiClientService Exception")
        {
            Source = "ApiClientService";
        }
        public ApiClientServiceException(string message) : base(message)
        {
            Source = "ApiClientService";
        }
    }
}
