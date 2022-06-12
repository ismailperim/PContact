namespace Report.Service.Interfaces
{
    public interface IReportService : IDisposable
    {
        Guid AddReportRequest(string location);
        List<Models.Report> GetAllReports(int pageRowCount = 10, int pageNumber = 0);
        Models.Report GetReportByID(Guid reportID);
    }
}
