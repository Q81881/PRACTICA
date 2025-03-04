using PollsApp.DTOs;
using PollsApp.Models;

namespace PollsApp.Interfaces
{
    public interface IPollService
    {
        Task<IEnumerable<Poll>> GetPollsAsync();
        Task<Poll> GetPollByIdAsync(int id);
        Task<Poll> CreatePollAsync(PollDTO pollDto);
    }
}
