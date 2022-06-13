using Core.Exceptions.Service;
using Core.Models;
using Core.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace Core.Services
{
    public class ApiClientService : IApiClientService
    {
        private readonly string _baseUrl = "";

        public ApiClientService(IOptions<ServiceOptions> options)
        {
            _baseUrl = options?.Value?.ContactApiOptions?.BaseUrl;
        }
        

        public async Task<T> Get<T>(string url, string? parameters)
        {
            #region --Validations--
            if (string.IsNullOrEmpty(_baseUrl))
                throw new ApiClientServiceException("BaseUrl null");

            if(string.IsNullOrEmpty(url))
                throw new ApiClientServiceException("Url null");
            #endregion

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                var result = await client.GetAsync(url);
                result.EnsureSuccessStatusCode();
                string resultContentString = await result.Content.ReadAsStringAsync();
                T resultContent = JsonConvert.DeserializeObject<T>(resultContentString);
                return resultContent;
            }
        }

        public  async Task Post<T>(string url, T contentValue)
        {
            #region --Validations--
            if (string.IsNullOrEmpty(_baseUrl))
                throw new ApiClientServiceException("BaseUrl null");

            if (string.IsNullOrEmpty(url))
                throw new ApiClientServiceException("Url null");
            #endregion
          
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");
                var result = await client.PostAsync(url, content);
                result.EnsureSuccessStatusCode();
            }
        }

        public  async Task Put<T>(string url, T stringValue)
        {
            #region --Validations--
            if (string.IsNullOrEmpty(_baseUrl))
                throw new ApiClientServiceException("BaseUrl null");

            if (string.IsNullOrEmpty(url))
                throw new ApiClientServiceException("Url null");
            #endregion
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                var content = new StringContent(JsonConvert.SerializeObject(stringValue), Encoding.UTF8, "application/json");
                var result = await client.PutAsync(url, content);
                result.EnsureSuccessStatusCode();
            }
        }
        
        public  async Task Delete(string url)
        {
            #region --Validations--
            if (string.IsNullOrEmpty(_baseUrl))
                throw new ApiClientServiceException("BaseUrl null");

            if (string.IsNullOrEmpty(url))
                throw new ApiClientServiceException("Url null");
            #endregion
            
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                var result = await client.DeleteAsync(url);
                result.EnsureSuccessStatusCode();
            }
        }        
    }
}
