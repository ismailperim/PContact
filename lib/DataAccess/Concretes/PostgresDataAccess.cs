using Core.Models;
using DataAccess.Interfaces;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace DataAccess.Concretes
{
    internal class PostgresDataAccess : IDataAccess
    {
        private string? _connectionString { get; set; }
        private readonly IDbConnection _dbConnection;
        private bool _disposed = false;
        public PostgresDataAccess(IOptions<ServiceOptions> options)
        {
            if (_dbConnection == null)
            {
                _connectionString = options.Value?.DatabaseOptions?.ConnectionString;
                _dbConnection = new NpgsqlConnection(_connectionString);
            }
        }
        public IDbConnection CreateConnection()
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();
            }
            return _dbConnection;
        }
        public IDbCommand CreateCommand(string commandText, CommandType commandType, IDbConnection connection)
        {
            return new NpgsqlCommand
            {
                CommandText = commandText,
                Connection = (NpgsqlConnection)connection,
                CommandType = commandType
            };
        }
        public IDataAdapter CreateAdapter(IDbCommand command)
        {
            return new NpgsqlDataAdapter((NpgsqlCommand)command);
        }
        public IDbDataParameter CreateParameter(IDbCommand command)
        {
            NpgsqlCommand SQLcommand = (NpgsqlCommand)command;
            return SQLcommand.CreateParameter();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _dbConnection.Close();
                _dbConnection.Dispose();
            }
            _disposed = true;
        }
    }
}
