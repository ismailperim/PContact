using static Report.Models.Enums;

namespace Report.Models
{
    public class Report
    {
        public Guid ID { get; set; }
        public DateTime CreateDate { get; set; }
        public ReportStatus Status { get; set; }

        public string Path { get; set; }

    }
}