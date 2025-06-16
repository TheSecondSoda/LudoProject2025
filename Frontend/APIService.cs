using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Frontend
{
    public class APIService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        // POST generic method
        public async Task PostJsonAsync<TRequest, TResponse>(
         string url,
         TRequest data,
         Action<TResponse> onSuccess,
         Action<Exception> onError)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<TResponse>(responseBody);
                onSuccess?.Invoke(result);
            }
            catch (Exception ex)
            {
                onError?.Invoke(ex);
            }
        }


        // GET generic method
        public async Task GetAsync<TResponse>(string url, Action<TResponse> onSuccess,
         Action<Exception> onError)
        {
            try
            {
               
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var result = JsonSerializer.Deserialize<TResponse>(responseBody,options);
                onSuccess?.Invoke(result);
            }
            catch (Exception ex)
            {

                onError?.Invoke(ex);
            }
            
        }
    }
}
