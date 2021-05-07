using System.Threading.Tasks;
using Website.Shared.Models;

namespace Website.Services.Interfaces
{
    public interface IResumeService
    {
        Task<ResumeModel> GetResume();
    }
}
