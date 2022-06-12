namespace Core.Exceptions.Service
{
    public class ReportServiceException : ServiceException
    {
        public ReportServiceException() : base("Generic ReportService Exception")
        {
            Source = "ReportService";
        }
        public ReportServiceException(string message) : base(message)
        {
            Source = "ReportService";
        }
    }
}
