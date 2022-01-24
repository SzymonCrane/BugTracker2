using BugTracker2.Models;
using System.Threading.Tasks;

namespace BugTracker2.Interfaces
{
    public interface IBugRepository
    {
        Task<Bug> GetBugById(int id);
    }
}