using Microsoft.AspNetCore.Mvc;
using Report.Service.Interfaces;


namespace Report.API.Controllers
{
    [ApiController]
    public class ReportController : ControllerBase
    {
        public readonly IReportService _reportService;
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Gets Reports with paging
        /// </summary>
        /// <param name="pageRowCount">Row count for each page</param>
        /// <param name="pageNumber">Requested page number</param>
        /// <returns>Report list</returns>
        [HttpGet]
        [Route("v1/reports")]
        public IEnumerable<Models.Report> Get(int pageRowCount = 10, int pageNumber = 0)
        {
            return _reportService.GetAllReports(pageRowCount, pageNumber);
        }


        /// <summary>
        /// Gets a Report by UniqueID
        /// </summary>
        /// <param name="personID">UniqueID of Report</param>
        /// <returns>A Report</returns>
        [HttpGet]
        [Route("v1/reports/{reportID}")]
        public Models.Report GetByID(Guid reportID)
        {
            return _reportService.GetReportByID(reportID);
        }

        /// <summary>
        /// Adds a Report
        /// </summary>
        /// <param name="model">Report model</param>
        /// <returns>Created Report record</returns>
        [HttpPost]
        [Route("v1/reports")]
        public Models.Report Add(Models.Report model)
        {
            var reportID = _reportService.AddReportRequest(model.Location);

            return _reportService.GetReportByID(reportID);
        }
    }
}