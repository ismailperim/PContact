using Core.Exceptions.DataAccess;
using DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Report.Service.Interfaces;
using System.Data;

namespace Report.Service
{
    public class ReportService : IReportService
    {
        bool _disposed = false;
        IDBManager? _dbManager;

        public ReportService(IServiceProvider provider)
        {
            _dbManager = provider.GetService<IDBManager>();
        }

        /// <summary>
        /// Adds new Contact Report request to DB
        /// </summary>
        /// <param name="location">Location name</param>
        /// <returns>Created Report Request UniqueID</returns>
        /// <exception cref="DBManagerNullException">Throws when the DB manager null</exception>
        /// <exception cref="ArgumentNullException">Throws when model or model required properties null</exception>
        public Guid AddReportRequest(string location)
        {
            #region --Data Validations--
            if (_dbManager is null)
                throw new DBManagerNullException("ReportService");

            if (location is null)
                throw new ArgumentNullException("location");
            #endregion


            var parameters = new List<IDbDataParameter>();

            parameters.Add(_dbManager.CreateParameter(Constant.P_LOCATION, location, DbType.String));

            return Guid.Parse(_dbManager.GetScalarValue(Constant.SP_ADD_REPORT_REQUEST, parameters.ToArray()).ToString());
        }

        public Models.Report GetReportByID(Guid reportID)
        {
            throw new NotImplementedException();
        }

        public List<Models.Report> GetReports()
        {
            throw new NotImplementedException();
        }

        #region -- Dispose --
        ~ReportService() => Dispose(false);
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
