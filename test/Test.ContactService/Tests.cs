using Core;
using Core.Models;
using DataAccess.Concretes;
using DataAccess.Interfaces;
using DataAccess.Manager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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

            _provider = services.BuildServiceProvider();


        }

        [Test]
        public void Add_Person_Without_ContactInfo()
        {
            var service = new Contact.Service.ContactService(_provider);
            var person = new Contact.Models.Person() { Name = "�smail", Surname = "Perim", Company = "Perim Inc." };

            service.AddPerson(person);

            Assert.Pass();
        }

        [Test]
        public void Add_Person_With_ContactInfo()
        {
            var service = new Contact.Service.ContactService(_provider);
            var person = new Contact.Models.Person() { Name = "�smail", Surname = "Perim", Company = "Perim Inc." };

            person.ContactInfos = new List<Contact.Models.ContactInfo>();
            person.ContactInfos.Add(new Contact.Models.ContactInfo() { Type = Contact.Models.Enums.ContactType.Phone, Value = "+905551231212" });
            person.ContactInfos.Add(new Contact.Models.ContactInfo() { Type = Contact.Models.Enums.ContactType.Email, Value = "ismail@perim.net" });
            person.ContactInfos.Add(new Contact.Models.ContactInfo() { Type = Contact.Models.Enums.ContactType.Location, Value = "�zmir" });


            service.AddPerson(person);

            Assert.Pass();
        }

        [Test]
        public void Add_Person_With_MissingParams()
        {
            var service = new Contact.Service.ContactService(_provider);
            var person = new Contact.Models.Person() { Name = "�smail", Surname = "Perim" };

            try
            {
                service.AddPerson(person);
            }
            catch (ArgumentNullException)
            {
                Assert.Pass();
            }


            Assert.Fail();
        }


        [Test]
        public void Add_ContactInfo()
        {
            var service = new Contact.Service.ContactService(_provider);
            var person = new Contact.Models.Person() { Name = "�smail", Surname = "Perim", Company = "Perim Inc." };

            var personID = service.AddPerson(person);


            service.AddContactInfo(personID, new Contact.Models.ContactInfo()
            {
                Type = Contact.Models.Enums.ContactType.Location,
                Value = "�zmir"
            });

            Assert.Pass();
        }

        [Test]
        public void Add_ContactInfo_With_MissingParams()
        {
            var service = new Contact.Service.ContactService(_provider);
            var person = new Contact.Models.Person() { Name = "�smail", Surname = "Perim", Company = "Perim Inc." };

            var personID = service.AddPerson(person);

            try
            {
                service.AddContactInfo(personID, new Contact.Models.ContactInfo()
                {
                    Type = Contact.Models.Enums.ContactType.Location,
                });
            }
            catch (ArgumentNullException)
            {

                Assert.Pass();
            }

            Assert.Fail();
        }

        [Test]
        public void Get_All_Persons()
        {
            var service = new Contact.Service.ContactService(_provider);

            var list = service.GetAllPersons();

            Assert.IsTrue(list.Count > 0);
        }

        [Test]
        public void Get_Person_By_ID()
        {
            var service = new Contact.Service.ContactService(_provider);
            var person = new Contact.Models.Person() { Name = "�smail", Surname = "Perim", Company = "Perim Inc." };

            var personID = service.AddPerson(person);


            service.AddContactInfo(personID, new Contact.Models.ContactInfo()
            {
                Type = Contact.Models.Enums.ContactType.Location,
                Value = "�zmir"
            });

            Contact.Models.Person newPerson = service.GetPersonByID(personID);


            Assert.IsTrue(personID == newPerson.ID && newPerson.ContactInfos.Count > 0 && newPerson.ContactInfos[0].Value == "�zmir" && newPerson.Name == "�smail");
        }


        [Test]
        public void Remove_Person()
        {
            var service = new Contact.Service.ContactService(_provider);
            var person = new Contact.Models.Person() { Name = "�smail", Surname = "Perim", Company = "Perim Inc." };

            var personID = service.AddPerson(person);

            service.AddContactInfo(personID, new Contact.Models.ContactInfo()
            {
                Type = Contact.Models.Enums.ContactType.Location,
                Value = "�zmir"
            });

            Assert.IsTrue(service.RemovePerson(personID));
        }



        [Test]
        public void Remove_ContactInfo()
        {
            var service = new Contact.Service.ContactService(_provider);
            var person = new Contact.Models.Person() { Name = "�smail", Surname = "Perim", Company = "Perim Inc." };

            var personID = service.AddPerson(person);

            service.AddContactInfo(personID, new Contact.Models.ContactInfo()
            {
                Type = Contact.Models.Enums.ContactType.Location,
                Value = "�zmir"
            });

            var newPerson = service.GetPersonByID(personID);

            Assert.IsTrue(service.RemoveContactInfo(personID, newPerson.ContactInfos[0].ID));
        }


        [Test]
        public void Get_Contact_Report()
        {
            var service = new Contact.Service.ContactService(_provider);

            var result = service.GetReport("�zmir");

            Assert.IsTrue(result.Location == "�zmir");
        }
    }
}