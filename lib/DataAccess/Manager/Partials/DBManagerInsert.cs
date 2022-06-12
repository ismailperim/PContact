using System;
using System.Data;
using System.Diagnostics;


namespace DataAccess.Manager
{
    public partial class DBManager
    {
        public void Insert(string commandText, IDbDataParameter[] parameters)
        {
            var connection = _database.CreateConnection();
            using (var command = _database.CreateCommand(commandText, CommandType.StoredProcedure, connection))
            {
                FillParameters(command);
                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                            command.Parameters.Add(parameter);
                    

                }
                command.ExecuteNonQuery();
                command.Parameters.Clear();
            }
        }

        public long Insert(string commandText, IDbDataParameter[] parameters, out long lastId)
        {
            lastId = 0;
            var connection = _database.CreateConnection();

            using (var command = _database.CreateCommand(commandText, CommandType.StoredProcedure, connection))
            {
                FillParameters(command, parameters);
                object newId = command.ExecuteScalar();
                lastId = Convert.ToInt64(newId);
            }

            return lastId;
        }
    }
}
