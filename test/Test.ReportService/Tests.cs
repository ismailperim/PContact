using Core;
using Core.Models;
using DataAccess.Concretes;
using DataAccess.Interfaces;
using DataAccess.Manager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Test.ReportService
{
    public class Tests
    {
        ServiceProvider _provider;

        [SetUp]
        public void Setup()
        {

            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<IOptions<ServiceOptions>, ServiceSettings>();
            services.AddTransient<IDBManager, DBManager>();
            services.AddTransient<IDataAccess, PostgresDataAccess>();

            _provider = services.BuildServiceProvider();


        }

        [Test]
        public void Add_Report_Request()
        {
            var service = new Report.Service.ReportService(_provider);

            service.AddReportRequest("Ýzmir");

            Assert.Pass();
        }
    }
}