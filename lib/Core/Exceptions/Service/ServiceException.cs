namespace Core.Exceptions.Service
{
    public class ServiceException : Exception
    {
        public ServiceException() : base("Generic Service Exception")
        {
            Source = "Service";
        }

        public ServiceException(string message) : base(message) {
            Source = "Service";
        }
    }
}
