using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDBManager : IDisposable
    {
        IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType);
        IDbDataParameter CreateParameter(string name, int size, object value, DbType dbType, ParameterDirection direction);
        IDbDataParameter CreateParameter(string name, object value, DbType dbType);
        void Delete(string commandText, IDbDataParameter[]? parameters = null);
        IDataReader GetDataReader(string commandText, IDbDataParameter[]? parameters = null);
        IDataReader GetDataReaderQuery(string commandText, IDbDataParameter[]? parameters = null);
        DataSet GetDataSet(string commandText, IDbDataParameter[]? parameters = null);
        DataTable GetDataTable(string commandText, IDbDataParameter[]? parameters = null);
        object GetScalarValue(string commandText, IDbDataParameter[]? parameters = null);
        void Insert(string commandText, IDbDataParameter[] parameters);
        long Insert(string commandText, IDbDataParameter[] parameters, out long lastInsertedID);
        void Update(string commandText, IDbDataParameter[] parameters);

    }
}