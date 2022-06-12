using Core.Exceptions.DataAccess;
using DataAccess.Helpers;
using DataAccess.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Report.Service.Interfaces;
using System.Data;
using static Report.Models.Enums;

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

        /// <summary>
        /// Gets all Contact Report requests
        /// </summary>
        /// <param name="pageRowCount">Row count for each page</param>
        /// <param name="pageNumber">Requested page number</param>
        /// <returns></returns>
        /// <exception cref="DBManagerNullException">Throws when the DB manager null</exception>
        public List<Models.Report> GetAllReports(int pageRowCount = 10, int pageNumber = 0)
        {
            #region --Data Validations--
            if (_dbManager is null)
                throw new DBManagerNullException("ReportService");
            #endregion

            IDataReader reader = null;
            List<Models.Report> list = new List<Models.Report>();
            try
            {
                var parameters = new List<IDbDataParameter>();
                parameters.Add(_dbManager.CreateParameter(Constant.P_PAGE_ROW_COUNT, pageRowCount, DbType.Int32));
                parameters.Add(_dbManager.CreateParameter(Constant.P_PAGE_NUMBER, pageNumber, DbType.Int32));

                reader = _dbManager.GetDataReader(Constant.SP_GET_ALL_REPORTS, parameters.ToArray());
                while (reader.Read())
                {
                    Models.Report model = new Models.Report();
                    model.ID = reader.GetDynamicValue<Guid>(Constant.C_ID);
                    model.Location = reader.GetDynamicValue<string>(Constant.C_LOCATION);
                    model.Status = (ReportStatus)reader.GetDynamicValue<short>(Constant.C_STATUS);
                    model.Path = reader.GetDynamicValue<string>(Constant.C_PATH);
                    model.CreateDate = reader.GetDynamicValue<DateTime>(Constant.C_CREATE_DATE);

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
