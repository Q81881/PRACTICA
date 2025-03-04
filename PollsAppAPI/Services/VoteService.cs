using System;
using System.Linq;
using System.Threading.Tasks;
using PollsApp.DTOs;
using PollsApp.Interfaces;
using PollsApp.Models;

namespace PollsApp.Services
{
    public class VoteService : IVoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPollRepository _pollRepository;

        public VoteService(IVoteRepository voteRepository, IUserRepository userRepository, IPollRepository pollRepository)
        {
            _voteRepository = voteRepository;
            _userRepository = userRepository;
            _pollRepository = pollRepository;
        }

        public async Task<Vote> VoteAsync(VoteDTO voteDto, string username)
        {
            var poll = await _pollRepository.GetPollByIdAsync(voteDto.PollId);
            if (poll == null)
                throw new Exception("Poll not found.");

            if (poll.Options == null || !poll.Options.Any())
                throw new Exception("Poll has no options available.");

            var user = await _userRepository.GetUserByUsernameAsync(username);
            if (user == null)
                throw new Exception("User not found.");

            var existingVote = await _voteRepository.GetVoteByUserAndPollAsync(user.Id, voteDto.PollId);
            if (existingVote != null)
            {
                throw new Exception("Вы уже голосовали в этом опросе.");
            }

            if (!poll.Options.Any(o => o.Id == voteDto.OptionId))
            {
                var validOptions = string.Join(", ", poll.Options.Select(o => o.Id));
                throw new Exception($"Invalid option. Valid options for this poll are: {validOptions}");
            }

            var vote = new Vote
            {
                PollId = voteDto.PollId,
                OptionId = voteDto.OptionId,
                UserId = user.Id
            };

            return await _voteRepository.AddVoteAsync(vote);
        }
    }
}
