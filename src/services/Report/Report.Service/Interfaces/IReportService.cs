using Report.Models;

namespace Report.Service.Interfaces
{
    public interface IReportService
    {
        Guid AddReportRequest(string location);
        ReportResult GetReportResults(string location);
        List<Models.Report> GetReports();
        Models.Report GetReportByID(Guid reportID);
    }
}
