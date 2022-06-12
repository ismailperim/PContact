using Contact.Models;

namespace Contact.Service.Interfaces
{
    public interface IContactService : IDisposable
    {
        Guid AddPerson(Person model);
        bool RemovePerson(Guid personID);
        Guid AddContactInfo(Guid personID, ContactInfo model);
        bool RemoveContactInfo(Guid personID, Guid contactInfoID);
        List<Person> GetAllPersons(int pageRowCount = 10, int pageNumber = 0);
        Person GetPersonByID(Guid personID);
    }
}
