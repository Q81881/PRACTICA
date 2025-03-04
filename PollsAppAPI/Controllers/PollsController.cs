using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PollsApp.DTOs;
using PollsApp.Interfaces;

namespace PollsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollsController : ControllerBase
    {
        private readonly IPollService _pollService;

        public PollsController(IPollService pollService)
        {
            _pollService = pollService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPolls()
        {
            var polls = await _pollService.GetPollsAsync();
            // polls уже содержит Options[], у каждой option — VotesCount
            return Ok(polls);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPoll(int id)
        {
            var poll = await _pollService.GetPollByIdAsync(id);
            if (poll == null)
                return NotFound();

            return Ok(poll);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreatePoll([FromBody] PollDTO pollDto)
        {
            var poll = await _pollService.CreatePollAsync(pollDto);
            return CreatedAtAction(nameof(GetPoll), new { id = poll.Id }, poll);
        }
    }
}
