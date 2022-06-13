using Core.Models;
using Core.Services.Interfaces;
using EasyNetQ;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Report.Models;

namespace Report.Service
{
    public class ReportQueueService : IHostedService, IDisposable
    {
        public readonly IBus _rabbitMQBus;
        public readonly IApiClientService _apiClient;


        public ReportQueueService(IOptions<ServiceOptions> options, IApiClientService apiClient)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            
            _apiClient = apiClient;

            _rabbitMQBus = RabbitHutch.CreateBus(options.Value.MessageQueueOptions?.ConnectionString);
            _rabbitMQBus.Advanced.QueueDeclare(Constant.Q_REPORT);
        }
        public Task StartAsync(CancellationToken stoppingToken)
        {
            _rabbitMQBus.SendReceive.ReceiveAsync<Models.Report>(Constant.Q_REPORT, async (x) =>
            {
                var result = await _apiClient.Get<ContactReport>("v1/report", $"location={x.Location}");

                using (var package = new ExcelPackage())
                {
                    var sheet = package.Workbook.Worksheets.Add("ContactReport");

                    var columns = new List<string>() { "Location", "PersonCount", "PhoneCount" };
                    int currentRow = 1;
                    
                    for (int i = 0; i < columns.Count; i++)
                    {
                        var currentColumn = i + 1;
                        sheet.Cells[currentRow, currentColumn].Value = columns[i];
                        sheet.Cells[currentRow, currentColumn].Style.Font.Bold = true;
                        sheet.Cells[currentRow, currentColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells[currentRow, currentColumn].Style.Fill.BackgroundColor.SetColor(1, 0, 88, 239);
                        sheet.Cells[currentRow, currentColumn].Style.Font.Color.SetColor(1, 255, 240, 239);
                    }

                    currentRow++;

                    sheet.Cells[currentRow, 1].Value = result.Location;
                    sheet.Cells[currentRow, 2].Value = result.PersonCount;
                    sheet.Cells[currentRow, 3].Value = result.PhoneCount;


                    sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
                    sheet.Cells[sheet.Dimension.Address].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[sheet.Dimension.Address].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[sheet.Dimension.Address].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[sheet.Dimension.Address].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;

                    await package.SaveAsAsync(new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}/Upload/{x.ID}.xlsx"));
                }


            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _rabbitMQBus?.Dispose();
        }
    }
}
