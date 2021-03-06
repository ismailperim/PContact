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

        /// <summary>
        /// Gets a Person by UniqueID
        /// </summary>
        /// <param name="personID">UniqueID of Person</param>
        /// <returns>Person model with ContactInfos</returns>
        /// <exception cref="DBManagerNullException">Throws when the DB manager null</exception>
        public Person GetPersonByID(Guid personID)
        {
            #region --Data Validations--
            if (_dbManager is null)
                throw new DBManagerNullException("ContactService");
            #endregion

            IDataReader reader = null;
            Person model = new Person();
            try
            {
                var parameters = new List<IDbDataParameter>();
                parameters.Add(_dbManager.CreateParameter(Constant.P_PERSON_ID, personID, DbType.Guid));


                reader = _dbManager.GetDataReader(Constant.SP_GET_PERSON_BY_ID, parameters.ToArray());
                while (reader.Read())
                {
                    model.ID = reader.GetDynamicValue<Guid>(Constant.C_ID);
                    model.Name = reader.GetDynamicValue<string>(Constant.C_NAME);
                    model.Surname = reader.GetDynamicValue<string>(Constant.C_SURNAME);
                    model.Company = reader.GetDynamicValue<string>(Constant.C_COMPANY);

                    string contactInfoJson = reader.GetDynamicValue<string>(Constant.C_CONTACT_INFO);

                    if (!string.IsNullOrEmpty(contactInfoJson))
                        model.ContactInfos = JsonConvert.DeserializeObject<List<ContactInfo>>(contactInfoJson);
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

            return model;
        }

        /// <summary>
        /// Removes a Contact Info from a Person by ID
        /// </summary>
        /// <param name="personID">UniqueID of Person</param>
        /// <param name="contactInfoID">UniqueID of Contact Info</param>
        /// <returns></returns>
        /// <exception cref="DBManagerNullException">Throws when the DB manager null</exception>
        public bool RemoveContactInfo(Guid personID, Guid contactInfoID)
        {
            #region --Data Validations--
            if (_dbManager is null)
                throw new DBManagerNullException("ContactService");
            #endregion

            var parameters = new List<IDbDataParameter>();
            parameters.Add(_dbManager.CreateParameter(Constant.P_PERSON_ID, personID, DbType.Guid));
            parameters.Add(_dbManager.CreateParameter(Constant.P_CONTACT_INFO_ID, contactInfoID, DbType.Guid));

            _dbManager.Insert(Constant.SP_REMOVE_CONTACT_INFO, parameters.ToArray());

            return true;
        }

        /// <summary>
        /// Removes a Person by UniqueID
        /// </summary>
        /// <param name="personID">UniqueID of Person</param>
        /// <returns>Action result</returns>
        /// <exception cref="DBManagerNullException">Throws when the DB manager null</exception>
        public bool RemovePerson(Guid personID)
        {
            #region --Data Validations--
            if (_dbManager is null)
                throw new DBManagerNullException("ContactService");
            #endregion

            var parameters = new List<IDbDataParameter>();
            parameters.Add(_dbManager.CreateParameter(Constant.P_PERSON_ID, personID, DbType.Guid));

            _dbManager.Insert(Constant.SP_REMOVE_PERSON, parameters.ToArray());

            return true;
        }


        /// <summary>
        /// Gets Contact Report result
        /// </summary>
        /// <param name="location">Location</param>
        /// <returns>Contact Report</returns>
        /// <exception cref="DBManagerNullException">Throws when the DB manager null</exception>
        public ContactReport GetReport(string location)
        {
            #region --Data Validations--
            if (_dbManager is null)
                throw new DBManagerNullException("ContactService");
            #endregion

            IDataReader reader = null;
            ContactReport model = new ContactReport();
            try
            {
                var parameters = new List<IDbDataParameter>();
                parameters.Add(_dbManager.CreateParameter(Constant.P_LOCATION, location, DbType.String));


                reader = _dbManager.GetDataReader(Constant.SP_GET_CONTACT_REPORT, parameters.ToArray());
                while (reader.Read())
                {
                    model.Location = reader.GetDynamicValue<string>(Constant.C_LOCATION);
                    model.PersonCount = reader.GetDynamicValue<int>(Constant.C_PERSON_COUNT);
                    model.PhoneCount = reader.GetDynamicValue<int>(Constant.C_PHONE_COUNT);
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

            return model;
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
