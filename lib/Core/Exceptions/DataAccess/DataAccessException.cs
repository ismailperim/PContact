namespace Core.Exceptions.DataAccess
{
    public class DataAccessException : Exception
    {
        public DataAccessException() : base("Generic DataAccess Exception")
        {
            Source = "DataAccess";
        }
    }
}
