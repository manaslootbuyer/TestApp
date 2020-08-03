using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MvvmAspire;
using test.Models;

namespace test.Services
{
    public class DiaryService : IDiaryService
    {
        private readonly IDataService _dataService;
        public DiaryService(IDataService dataService)
        {
            _dataService = dataService;
           
        }


        public async Task<UserModel> AddDiaryAsync(UserCommand request, CancellationToken cts = default(CancellationToken))
        {
            var response = await _dataService.GetResponseAsync<UserModel>("users", cts, WebRequestMethod.POST, request);
            return response;
        }
    }
}