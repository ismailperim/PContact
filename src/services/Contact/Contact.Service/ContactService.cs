using Contact.Models;
using Contact.Service.Interfaces;
using Core.Exceptions.DataAccess;
using DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Contact.Service
{
    /// <summary>
    /// Manages Contact actions with communication DB 
    /// </summary>
    public class ContactService : IContactService
    {
        bool _disposed = false;
        IDBManager? _dbManager;

        public ContactService(IServiceProvider provider)
        {
            _dbManager = provider.GetService<IDBManager>();
        }

        public Guid AddContactInfo(Guid personID, ContactInfo model)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds new Person to DB
        /// </summary>
        /// <param name="model">Person model</param>
        /// <returns>Created Person UUID</returns>
        /// <exception cref="DBManagerNullException">Throws when de DB manager null</exception>
        public Guid AddPerson(Person model)
        {
            if (_dbManager == null)
                throw new DBManagerNullException("ContactService");


            var parameters = new List<IDbDataParameter>();

            //parameters.Add(_dbManager.CreateParameter(ParameterNames.SubscriberID, model.SubscriberID, DbType.Int64));


            _dbManager.Insert("public.sp_add_person", parameters.ToArray());


            return default;
        }

        public List<Person> GetAllPersons()
        {
            throw new NotImplementedException();
        }

        public Person GetPersonByID(Guid personID)
        {
            throw new NotImplementedException();
        }

        public bool RemoveContactInfo(Guid personID, Guid contactInfoID)
        {
            throw new NotImplementedException();
        }

        public bool RemovePerson(Guid personID)
        {
            throw new NotImplementedException();
        }


        #region -- Dispose --
        ~ContactService() => Dispose(false);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
                _dbManager?.Dispose();

            _disposed = true;
        }
        #endregion
    }
}
