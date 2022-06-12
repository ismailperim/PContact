﻿using Contact.Models;
using Contact.Service.Interfaces;
using Core.Exceptions.DataAccess;
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
        /// <exception cref="DBManagerNullException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
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
        /// <exception cref="DBManagerNullException">Throws when de DB manager null</exception>
        /// <exception cref="ArgumentNullException">Throws when de DB manager null</exception>
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