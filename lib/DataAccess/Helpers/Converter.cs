using System.Data;

namespace DataAccess.Helpers
{
    public static class Converter
    {
        /// <summary>
        /// Gets DataReader field with requested Type
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="value">DataReader</param>
        /// <param name="fieldName">Field Name</param>
        /// <returns></returns>
        public static T GetDynamicValue<T>(this IDataReader value, string fieldName)
        {
            if (value[fieldName] == DBNull.Value || value[fieldName] == null) return default(T);
            else
            {
                Type t = Nullable.GetUnderlyingType(value[fieldName].GetType()) ?? value[fieldName].GetType();

                return (T)Convert.ChangeType(value[fieldName], t);

            }
        }

    }
}
