namespace Report.Service.Interfaces
{
    public interface IReportService
    {
        Guid AddReportRequest(string location);
        List<Models.Report> GetReports();
        Models.Report GetReportByID(Guid reportID);
    }
}
