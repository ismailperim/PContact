namespace Core.Exceptions.DataAccess
{
    public class ConnectionStringNullException : Exception
    {
        public ConnectionStringNullException() : base("ConnectionString null")
        {
            Source = "DataAccess";
        }
    }
}
