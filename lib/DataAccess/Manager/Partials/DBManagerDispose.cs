using System;

namespace DataAccess.Manager
{
    public partial class DBManager
    {
        private bool _disposed = false;

        ~DBManager() => Dispose(false);
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
                _database.Dispose();
            }
            _disposed = true;
        }
    }
}
