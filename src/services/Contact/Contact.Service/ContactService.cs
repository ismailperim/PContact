using Contact.Models;
using Contact.Service.Interfaces;
using Core.Exceptions.DataAccess;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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
        /// <summary>
        /// Adds new Contact Information to existing Person
        /// </summary>
        /// <param name="personID">Person UniqueID</param>
        /// <param name="model">ContactInfo model</param>
        /// <returns>Created Contact Information UniqueID</returns>
        /// <exception cref="DBManagerNullException">Throws when the DB manager null</exception>
        /// <exception cref="ArgumentNullException">Throws when model or model required properties null</exception>
        public Guid AddContactInfo(Guid personID, ContactInfo model)
        {
            #region --Data Validations--
            if (_dbManager is null)
                throw new DBManagerNullException("ContactService");

            if (model is null)
                throw new ArgumentNullException("model");

            if (model.Value is null)
                throw new ArgumentNullException("model.Value");
            #endregion


            var parameters = new List<IDbDataParameter>();

            parameters.Add(_dbManager.CreateParameter(Constant.P_PERSON_ID, personID, DbType.Guid));
            parameters.Add(_dbManager.CreateParameter(Constant.P_TYPE, (short)model.Type, DbType.Int16));
            parameters.Add(_dbManager.CreateParameter(Constant.P_VALUE, model.Value, DbType.String));



            return Guid.Parse(_dbManager.GetScalarValue(Constant.SP_ADD_CONTACT_INFO, parameters.ToArray()).ToString());
        }

        /// <summary>
        /// Adds new Person to DB
        /// </summary>
        /// <param name="model">Person model</param>
        /// <returns>Created Person UniqueID</returns>
        /// <exception cref="DBManagerNullException">Throws when the DB manager null</exception>
        /// <exception cref="ArgumentNullException">Throws when model or model required properties null</exception>
        public Guid AddPerson(Person model)
        {
            #region --Data Validations--
            if (_dbManager is null)
                throw new DBManagerNullException("ContactService");

            if (model is null)
                throw new ArgumentNullException("model");

            if (model.Name is null)
                throw new ArgumentNullException("model.Name");

            if (model.Surname is null)
                throw new ArgumentNullException("model.Surname");

            if (model.Company is null)
                throw new ArgumentNullException("model.Company");
            #endregion


            var parameters = new List<IDbDataParameter>();

            parameters.Add(_dbManager.CreateParameter(Constant.P_NAME, model.Name, DbType.String));
            parameters.Add(_dbManager.CreateParameter(Constant.P_SURNAME, model.Surname, DbType.String));
            parameters.Add(_dbManager.CreateParameter(Constant.P_COMPANY, model.Company, DbType.String));
            parameters.Add(_dbManager.CreateParameter(Constant.P_CONTACT_INFOS, model.ContactInfos != null ? JsonConvert.SerializeObject(model.ContactInfos) : DBNull.Value, DbType.Object));


            return Guid.Parse(_dbManager.GetScalarValue(Constant.SP_ADD_PERSON, parameters.ToArray()).ToString());
        }


        /// <summary>
        /// Gets all Person with paging
        /// </summary>
        /// <param name="pageRowCount">Row count for each page</param>
        /// <param name="pageNumber">Page number</param>
        /// <returns>List of Person for requested page</returns>
        /// <exception cref="DBManagerNullException">Throws when the DB manager null</exception>
        public List<Person> GetAllPersons(int pageRowCount = 10, int pageNumber = 0)
        {
            #region --Data Validations--
            if (_dbManager is null)
                throw new DBManagerNullException("ContactService");
            #endregion

            
            IDataReader reader = null;
            List<Person> list = new List<Person>();
            try
            {
                var parameters = new List<IDbDataParameter>();
               parameters.Add(_dbManager.CreateParameter(Constant.P_PAGE_ROW_COUNT, pageRowCount, DbType.Int32));
               parameters.Add(_dbManager.CreateParameter(Constant.P_PAGE_NUMBER, pageNumber, DbType.Int32));

                reader = _dbManager.GetDataReader(Constant.SP_GET_ALL_PERSONS, parameters.ToArray());
                while (reader.Read())
                {
                    Person model = new Person();
                    model.ID = reader.GetDynamicValue<Guid>(Constant.C_ID);
                    model.Name = reader.GetDynamicValue<string>(Constant.C_NAME);
                    model.Surname = reader.GetDynamicValue<string>(Constant.C_SURNAME);
                    model.Company = reader.GetDynamicValue<string>(Constant.C_COMPANY);

                    list.Add(model);
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }

            return list;

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
