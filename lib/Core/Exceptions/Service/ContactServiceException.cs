namespace Core.Exceptions.Service
{
    public class ContactServiceException : ServiceException
    {
        public ContactServiceException() : base("Generic ContactService Exception")
        {
            Source = "ContactService";
        }
        public ContactServiceException(string message) : base(message)
        {
            Source = "ContactService";
        }
    }
}
