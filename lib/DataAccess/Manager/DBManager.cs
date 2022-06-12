using DataAccess.Interfaces;
using System.Data;

namespace DataAccess.Manager
{
    public partial class DBManager : IDBManager
    {
        private IDataAccess _database;
        public DBManager(IDataAccess dataAccess)
        {
            _database = dataAccess;
        }
        private static void FillParameters(IDbCommand command, IDbDataParameter[] parameters = null)
        {
            if (parameters != null)
                foreach (var parameter in parameters)
                    command.Parameters.Add(parameter);
        }
    }
}
