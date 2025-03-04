using Microsoft.EntityFrameworkCore;
using PollsApp.Interfaces;
using PollsApp.Models;

namespace PollsApp.Repositories
{
    public class PollRepository : IPollRepository
    {
        private readonly ApplicationDbContext _context;

        public PollRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Poll>> GetAllPollsAsync()
        {
            // Загружаем опросы вместе с вариантами и голосами
            var polls = await _context.Polls
                .Include(p => p.Options)
                .Include(p => p.Votes)
                .ToListAsync();

            // Проставляем VotesCount для каждой опции
            foreach (var poll in polls)
            {
                foreach (var option in poll.Options)
                {
                    option.VotesCount = poll.Votes.Count(v => v.OptionId == option.Id);
                }
            }

            return polls;
        }

        public async Task<Poll> GetPollByIdAsync(int id)
        {
            // Аналогично, подгружаем опрос вместе с вариантами и голосами
            var poll = await _context.Polls
                .Include(p => p.Options)
                .Include(p => p.Votes)
                .FirstOrDefaultAsync(p => p.Id == id);

            // Проставляем VotesCount
            if (poll != null)
            {
                foreach (var option in poll.Options)
                {
                    option.VotesCount = poll.Votes.Count(v => v.OptionId == option.Id);
                }
            }

            return poll;
        }

        public async Task<Poll> AddPollAsync(Poll poll)
        {
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();
            return poll;
        }

        public async Task UpdatePollAsync(Poll poll)
        {
            _context.Polls.Update(poll);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePollAsync(int id)
        {
            var poll = await _context.Polls.FindAsync(id);
            if (poll != null)
            {
                _context.Polls.Remove(poll);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Vote?> GetVoteByUserAndPollAsync(int userId, int pollId)
        {
            return await _context.Votes
                .FirstOrDefaultAsync(v => v.UserId == userId && v.PollId == pollId);
        }
    }
}
