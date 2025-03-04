using Microsoft.EntityFrameworkCore;
using PollsApp.Interfaces;
using PollsApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PollsApp.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private readonly ApplicationDbContext _context;

        public VoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Vote> AddVoteAsync(Vote vote)
        {
            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();
            return vote;
        }

        public async Task<Vote?> GetVoteByUserAndPollAsync(int userId, int pollId)
        {
            return await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == userId && v.PollId == pollId);
        }

        public async Task<IEnumerable<Vote>> GetVotesByPollIdAsync(int pollId)
        {
            return await _context.Votes
                .Where(v => v.PollId == pollId)
                .ToListAsync();
        }
    }
}
