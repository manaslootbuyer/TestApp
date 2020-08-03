using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MvvmAspire;

namespace test.Services
{
    public class DiaryService : IDiaryService
    {
        private readonly IDataService _dataService;
        public DiaryService(IDataService dataService)
        {
            _dataService = dataService;
           
        }
    }
}