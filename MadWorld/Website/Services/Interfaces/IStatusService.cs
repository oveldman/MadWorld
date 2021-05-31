using System;
using System.Threading.Tasks;

namespace Website.Services.Interfaces
{
    public interface IStatusService
    {
        Task<bool> CheckStatus();
        Task<bool> CheckDatabaseAuthentication();
        Task<bool> CheckDatabaseMadWorld();
    }
}
