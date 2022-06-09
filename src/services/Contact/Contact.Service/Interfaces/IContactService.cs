using Contact.Models;

namespace Contact.Service.Interfaces
{
    public interface IContactService
    {
        Guid AddPerson(Person model);
        bool RemovePerson(Guid personID);
        Guid AddContactInfo(Guid personID, ContactInfo model);
        bool RemoveContactInfo(Guid personID, Guid contactInfoID);
        List<Person> GetAllPersons();
        Person GetPersonByID(Guid personID);
    }
}
