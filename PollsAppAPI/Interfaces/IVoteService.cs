using PollsApp.DTOs;
using PollsApp.Models;
using System.Threading.Tasks;

namespace PollsApp.Interfaces
{
    public interface IVoteService
    {
        Task<Vote> VoteAsync(VoteDTO voteDto, string username);
    }
}
