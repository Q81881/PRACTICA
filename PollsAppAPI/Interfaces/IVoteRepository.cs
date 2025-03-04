using PollsApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PollsApp.Interfaces
{
    public interface IVoteRepository
    {
        Task<Vote> AddVoteAsync(Vote vote);
        Task<Vote?> GetVoteByUserAndPollAsync(int userId, int pollId); // Новый метод
        Task<IEnumerable<Vote>> GetVotesByPollIdAsync(int pollId);
    }
}
