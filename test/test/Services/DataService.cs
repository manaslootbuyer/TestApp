using System;
using System.Collections;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MvvmAspire;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using test.Models;

namespace test.Services
{
    public class DataService : IDataService
    {
        public event Action<object, Models.ErrorEventArgs> OnNewtorkError;

        public async Task<bool> GetSuccess(string endpointName, CancellationToken cts, WebRequestMethod requestMethod, object data = null)
        {
            var response = await GetResponse<Response>(endpointName, requestMethod, cts, data);

            return response != null;
        }

        public async Task<T> GetResponseAsync<T>(string endpointName, CancellationToken cts, WebRequestMethod requestMethod, object data = null) where T : class
        {
            try
            {
                var response = await GetResponse<T>(endpointName, requestMethod, cts, data);
                return response?.Data;
            }
            catch (ArgumentNullException aEx)
            {
                OnNewtorkError?.Invoke(this, new Models.ErrorEventArgs(ErrorType.BadRequest));
            }
            catch (InvalidOperationException iEx)
            {
                OnNewtorkError?.Invoke(this, new Models.ErrorEventArgs(ErrorType.BadRequest));
            }
            catch (TaskCanceledException)
            {
                OnNewtorkError?.Invoke(this, new Models.ErrorEventArgs(ErrorType.Timeout));
            }
            catch (HttpRequestException hEx)
            {
                var errorType = GetErrorType(hEx);
                OnNewtorkError?.Invoke(this, new Models.ErrorEventArgs(errorType));
            }
            catch (Exception ex)
            {
                //throw;
            }
            return null;
        }

        public ErrorType GetErrorType(HttpRequestException ex)
        {
            var errorType = ErrorType.Unkown;

            if (!int.TryParse(ex.Message, out var statusCode)) return errorType;

            if (statusCode == 401)
                errorType = ErrorType.Unauthorized;
            else if (statusCode >= 400 && statusCode < 500)
                errorType = ErrorType.BadRequest;
            else if (statusCode >= 500)
                errorType = ErrorType.ServerError;

            return errorType;
        }

        private async Task<Response<T>> GetResponse<T>(string endpointName, WebRequestMethod requestMethod, CancellationToken cts, object data)
        {
            var result = new Response<T>();
            var response = await GetResponse(endpointName, requestMethod, cts, data);

            if (!response.IsSuccessStatusCode) throw new HttpRequestException(((int)response.StatusCode).ToString());

            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (responseString == "[]" && !typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(typeof(T).GetTypeInfo()))
                    result.Data = default(T);
                else
                    result.Data = JsonConvert.DeserializeObject<T>(responseString);
            return result;
        }


        public async Task<string> GetRawStationsAsync(string endpointName, CancellationToken cts, WebRequestMethod requestMethod)
        {
            var response = await GetResponse(endpointName, requestMethod, cts,null);

            if (!response.IsSuccessStatusCode) throw new HttpRequestException(((int)response.StatusCode).ToString());

            try{
                return  await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (Exception){
                return "";
            }
        }

        private async Task<HttpResponseMessage> GetResponse(string endpointName, WebRequestMethod requestMethod, CancellationToken cts, object data)
        {
            using (var client = GetClient(endpointName))
            {
                switch (requestMethod)
                {
                    case WebRequestMethod.DELETE: return await client.DeleteAsync(string.Empty, cts);
                    case WebRequestMethod.GET: return await client.GetAsync(string.Empty, cts).ConfigureAwait(true);
                    case WebRequestMethod.POST: return await client.PostAsync(string.Empty, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"), cts);
                    case WebRequestMethod.PUT: return await client.PutAsync(string.Empty, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json"), cts);
                    default: throw new ArgumentOutOfRangeException();
                }
            }
        }

        private HttpClient GetClient(string url)
        {
            var httpHandler = new HttpClientHandler
            {
               // AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            var client = new HttpClient(httpHandler)
            {
                BaseAddress = new Uri(url)
            };

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            return client;
        }

        private class Response
        {
        }

        private class Response<T> : Response
        {
            public T Data { get; set; }
        }
    }
}