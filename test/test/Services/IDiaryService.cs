using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using test.Models;

namespace test.Services
{
    public interface IDiaryService
    {
        Task<UserModel> AddDiaryAsync(UserCommand request, CancellationToken cts = default(CancellationToken));
    }
}