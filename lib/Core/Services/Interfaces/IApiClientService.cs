namespace Core.Services.Interfaces
{
    public interface IApiClientService
    {
        Task<T> Get<T>(string url, string? parameters);
        Task Post<T>(string url, T contentValue);
        Task Put<T>(string url, T stringValue);
        Task Delete(string url);
    }
}
