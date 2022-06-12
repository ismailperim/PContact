namespace Core.Exceptions.DataAccess
{
    public class DBManagerNullException : Exception
    {
        public DBManagerNullException() : base("DBManager null")
        {
            Source = "DataAccess";
        }

        public DBManagerNullException(string source) : base("DBManager null")
        {
            Source = source;
        }
    }
}
