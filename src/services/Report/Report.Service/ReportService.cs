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

        /// <summary>
        /// Gets Report by UniqueID
        /// </summary>
        /// <param name="reportID">UniqueID of Report</param>
        /// <returns>Report with details</returns>
        /// <exception cref="DBManagerNullException">Throws when the DB manager null</exception>
        public Models.Report GetReportByID(Guid reportID)
        {
            #region --Data Validations--
            if (_dbManager is null)
                throw new DBManagerNullException("ReportService");
            #endregion

            IDataReader reader = null;
            Models.Report model = new Models.Report();
            try
            {
                var parameters = new List<IDbDataParameter>();
                parameters.Add(_dbManager.CreateParameter(Constant.P_REPORT_ID, reportID, DbType.Guid));


                reader = _dbManager.GetDataReader(Constant.SP_GET_REPORT_BY_ID, parameters.ToArray());
                while (reader.Read())
                {
                    model.ID = reader.GetDynamicValue<Guid>(Constant.C_ID);
                    model.Location = reader.GetDynamicValue<string>(Constant.C_LOCATION);
                    model.Status = (ReportStatus)reader.GetDynamicValue<short>(Constant.C_STATUS);
                    model.Path = reader.GetDynamicValue<string>(Constant.C_PATH);
                    model.CreateDate = reader.GetDynamicValue<DateTime>(Constant.C_CREATE_DATE);
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

        /// <summary>
        /// Updatesthe status of Report
        /// </summary>
        /// <param name="reportID">UniqueID of ReportRequest</param>
        /// <param name="path">Report file path</param>
        /// <exception cref="DBManagerNullException">Throws when the DB manager null</exception>
        public void UpdateReport(Guid reportID, string path)
        {
                #region --Data Validations--
                if (_dbManager is null)
                    throw new DBManagerNullException("ReportService");
                #endregion

                var parameters = new List<IDbDataParameter>();
                parameters.Add(_dbManager.CreateParameter(Constant.P_REPORT_ID, reportID, DbType.Guid));
                parameters.Add(_dbManager.CreateParameter(Constant.P_PATH, path, DbType.String));


            _dbManager.Insert(Constant.SP_UPDATE_REPORT, parameters.ToArray());
            
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
