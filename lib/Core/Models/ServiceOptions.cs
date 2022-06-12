namespace Core.Models
{
    public class ServiceOptions
    {
        public MessageQueueOptions? MessageQueueOptions { get; set; }
        public DatabaseOptions? DatabaseOptions { get; set; }
    }
}
