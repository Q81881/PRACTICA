using PollsApp.Models;

namespace PollsApp.Interfaces
{
    public interface IPollRepository
    {
        Task<IEnumerable<Poll>> GetAllPollsAsync();
        Task<Poll> GetPollByIdAsync(int id);
        Task<Poll> AddPollAsync(Poll poll);
        Task UpdatePollAsync(Poll poll);
        Task DeletePollAsync(int id);

        Task<Vote?> GetVoteByUserAndPollAsync(int userId, int pollId);
    }
}
