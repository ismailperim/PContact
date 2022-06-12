using System.Data;

namespace DataAccess.Manager
{
    public partial class DBManager
    {
        public void Delete(string commandText, IDbDataParameter[] parameters = null)
        {
            var connection = _database.CreateConnection();
            using (var command = _database.CreateCommand(commandText, CommandType.StoredProcedure, connection))
            {
                FillParameters(command, parameters);
                command.ExecuteNonQuery();
            }
        }
    }
}
