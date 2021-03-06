using DataAccess.Interfaces;
using Npgsql;
using System.Data;

namespace DataAccess.Parameter
{
    internal static class DataParameterManager
    {
        internal static IDbDataParameter CreateParameter(IDataAccess dataAccess, string name, object value, DbType dbType, ParameterDirection direction = ParameterDirection.Input)
        {
            if (dbType == DbType.Object)
                return new NpgsqlParameter
                {
                    NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb,
                    ParameterName = name,
                    Direction = direction,
                    Value = value
                };
            else
                return new NpgsqlParameter
                {
                    DbType = dbType,
                    ParameterName = name,
                    Direction = direction,
                    Value = value
                };
        }

        internal static IDbDataParameter CreateParameter(IDataAccess dataAccess, string name, int size, object value, DbType dbType, ParameterDirection direction = ParameterDirection.Input)
        {
            if (dbType == DbType.Object)
                return new NpgsqlParameter
                {
                    NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Jsonb,
                    Size = size,
                    ParameterName = name,
                    Direction = direction,
                    Value = value
                };
            else
                return new NpgsqlParameter
                {
                    DbType = dbType,
                    Size = size,
                    ParameterName = name,
                    Direction = direction,
                    Value = value
                };
        }

    }
}
