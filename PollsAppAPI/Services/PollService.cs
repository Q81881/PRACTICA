using PollsApp.DTOs;
using PollsApp.Interfaces;
using PollsApp.Models;

namespace PollsApp.Services
{
    public class PollService : IPollService
    {
        private readonly IPollRepository _pollRepository;

        public PollService(IPollRepository pollRepository)
        {
            _pollRepository = pollRepository;
        }

        public async Task<IEnumerable<Poll>> GetPollsAsync()
        {
            return await _pollRepository.GetAllPollsAsync();
        }

        public async Task<Poll> GetPollByIdAsync(int id)
        {
            return await _pollRepository.GetPollByIdAsync(id);
        }

        public async Task<Poll> CreatePollAsync(PollDTO pollDto)
        {
            var poll = new Poll
            {
                Title = pollDto.Title,
                Options = pollDto.Options.Select(o => new Option { Text = o }).ToList()
            };

            return await _pollRepository.AddPollAsync(poll);
        }
    }
}
