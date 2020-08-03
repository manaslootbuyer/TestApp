using System;
using System.Threading;
using System.Threading.Tasks;
using MvvmAspire;
using test.Models;

namespace test
{
    public interface IDataService
    {
        Task<bool> GetSuccess(string endpointName, CancellationToken cts, WebRequestMethod requestMethod, object data = null);
        Task<T> GetResponseAsync<T>(string endpointName, CancellationToken cts, WebRequestMethod requestMethod, object data = null) where T : class;
        Task<string> GetRawStationsAsync(string endpointName, CancellationToken cts,WebRequestMethod requestMethod);
        event Action<object, ErrorEventArgs> OnNewtorkError;
    }
}