using Contact.Models;
using Contact.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Contact.API.Controllers
{
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Gets Persons with paging
        /// </summary>
        /// <param name="pageRowCount">Row count for each page</param>
        /// <param name="pageNumber">Requested page number</param>
        /// <returns>Person list without Contact Infos</returns>
        [HttpGet]
        [Route("v1/persons")]
        public IEnumerable<Person> Get(int pageRowCount = 10, int pageNumber = 0)
        {
            return _contactService.GetAllPersons(pageRowCount, pageNumber);
        }

        /// <summary>
        /// Gets a Person by UniqueID
        /// </summary>
        /// <param name="personID">UniqueID of Person</param>
        /// <returns>A Person with Contact Info</returns>
        [HttpGet]
        [Route("v1/persons/{personID}")]
        public Person GetByID(Guid personID)
        {
            return _contactService.GetPersonByID(personID);
        }

        /// <summary>
        /// Adds a Person
        /// </summary>
        /// <param name="model">Person model</param>
        /// <returns>Created Person record with Contact Info</returns>
        [HttpPost]
        [Route("v1/persons")]
        public Person Add(Person model)
        {
            var personID = _contactService.AddPerson(model);

            return _contactService.GetPersonByID(personID);
        }

        /// <summary>
        /// Deletes a Person and Contact Infos by UniqueID
        /// </summary>
        /// <param name="personID">UniqueID of Person</param>
        /// <returns>A Person with Contact Info</returns>
        [HttpDelete]
        [Route("v1/persons/{personID}")]
        public bool Remove(Guid personID)
        {
            return _contactService.RemovePerson(personID);
        }


        /// <summary>
        /// Adds a Contact Info to a Person
        /// </summary>
        /// <param name="personID">Person UniqueID</param>
        /// <param name="model">Contact Info model</param>
        /// <returns></returns>
        [HttpPost]
        [Route("v1/persons/{personID}/contacts")]
        public ContactInfo AddContactInfo(Guid personID, ContactInfo model)
        {
            var contactInfoID = _contactService.AddContactInfo(personID, model);

            return _contactService.GetPersonByID(personID)?.ContactInfos?.Where(x => x.ID == contactInfoID)?.FirstOrDefault();
        }

        /// <summary>
        /// Deletes a Contact Info from a Person
        /// </summary>
        /// <param name="personID">UniqueID of Person</param>
        /// <param name="contactInfoID">UniqueID of Contact Info</param>
        /// <returns></returns>
        [HttpDelete]
        [Route("v1/persons/{personID}/contacts/{contactInfoID}")]
        public bool Remove(Guid personID, Guid contactInfoID)
        {
            return _contactService.RemoveContactInfo(personID, contactInfoID);
        }
        
        /// <summary>
        /// Gets Contact Report by Location
        /// </summary>
        /// <param name="location">Location Name</param>
        /// <returns>Contact Report model</returns>
        [HttpGet]
        [Route("v1/report")]
        public ContactReport GetReport(string location)
        {
            return _contactService.GetReport(location);
        }
    }
}