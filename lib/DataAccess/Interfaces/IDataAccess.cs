using System;
using System.Data;

namespace DataAccess.Interfaces
{
    public interface IDataAccess : IDisposable
    {
        IDbConnection CreateConnection();
        IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection);
        IDataAdapter CreateAdapter(IDbCommand command);
        IDbDataParameter CreateParameter(IDbCommand command);
    }
}
