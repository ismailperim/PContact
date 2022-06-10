using Core;
using Core.Models;
using DataAccess.Concretes;
using DataAccess.Interfaces;
using DataAccess.Manager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace Test.ContactService
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
            
            _provider =  services.BuildServiceProvider();


        }

        [Test]
        public void Test1()
        {
            var service = new Contact.Service.ContactService(_provider);

            service.AddPerson(new Contact.Models.Person());


            Assert.Pass();
        }
    }
}