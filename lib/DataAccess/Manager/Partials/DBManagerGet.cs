using System.Data;

namespace DataAccess.Manager
{
    public partial class DBManager
    {
        public DataSet GetDataSet(string commandText, IDbDataParameter[] parameters = null)
        {
            var connection = _database.CreateConnection();
            using (var command = _database.CreateCommand(commandText, CommandType.StoredProcedure, connection))
            {
                FillParameters(command, parameters);
                var dataset = new DataSet();
                var dataAdaper = _database.CreateAdapter(command);
                dataAdaper.Fill(dataset);
                return dataset;
            }
        }

        public IDataReader GetDataReader(string commandText, IDbDataParameter[] parameters = null)
        {
            var connection = _database.CreateConnection();

            var command = _database.CreateCommand(commandText, CommandType.StoredProcedure, connection);
            FillParameters(command, parameters);
            return command.ExecuteReader();
        }

        public IDataReader GetDataReaderQuery(string commandText, IDbDataParameter[] parameters = null)
        {
            var connection = _database.CreateConnection();

            var command = _database.CreateCommand(commandText, CommandType.Text, connection);
            FillParameters(command, parameters);
            return  command.ExecuteReader();
        }

        public DataTable GetDataTable(string commandText, IDbDataParameter[] parameters = null)
        {
            var connection = _database.CreateConnection();

            using (var command = _database.CreateCommand(commandText, CommandType.StoredProcedure, connection))
            {
                FillParameters(command, parameters);

                var dataset = new DataSet();
                var dataAdaper = _database.CreateAdapter(command);
                dataAdaper.Fill(dataset);

                return dataset.Tables[0];
            }
        }
        public object GetScalarValue(string commandText, IDbDataParameter[] parameters = null)
        {
            var connection = _database.CreateConnection();

            using (var command = _database.CreateCommand(commandText, CommandType.StoredProcedure, connection))
            {
                FillParameters(command, parameters);
                object obj = command.ExecuteScalar();
                connection.Close();
                return obj;
            }
        }
    }
}
