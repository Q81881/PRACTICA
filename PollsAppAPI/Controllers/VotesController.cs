using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PollsApp.DTOs;
using PollsApp.Hubs;
using PollsApp.Interfaces;

namespace PollsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVoteService _voteService;
        private readonly IHubContext<VoteHub> _hubContext;

        public VotesController(IVoteService voteService, IHubContext<VoteHub> hubContext)
        {
            _voteService = voteService;
            _hubContext = hubContext;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Vote([FromBody] VoteDTO voteDto)
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            var vote = await _voteService.VoteAsync(voteDto, username);

            // Оповещаем всех клиентов о новых результатах
            await _hubContext.Clients.All.SendAsync("ReceiveResults", voteDto.PollId);

            return Ok(vote);
        }
    }
}
