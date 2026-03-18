using Microsoft.AspNetCore.Mvc;
using VoteApi.Models;
using VoteApi.Repositories;

namespace VoteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly IVoteRepository _voteRepository;

        public VoteController(IVoteRepository voteRepository)
        {
            _voteRepository = voteRepository;
        }

        [HttpPost]
        public async Task<ActionResult<VoteItem>> CreateVote([FromBody] VoteItem voteItem)
        {
            if (voteItem == null)
            {
                return BadRequest("Vote item cannot be null");
            }

            var createdVote = await _voteRepository.CreateVoteAsync(voteItem);

            return CreatedAtAction(nameof(GetVoteById), new { id = createdVote.Id }, createdVote);
        }

        [HttpGet]
        public async Task<ActionResult> GetVotes([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1)
                page = 1;
            
            if (pageSize < 1)
                pageSize = 10;

            var (items, totalItems) = await _voteRepository.GetVotesAsync(page, pageSize);
            
            return Ok(new 
            {
                items,
                page,
                pageSize,
                totalItems,
                totalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VoteItem>> GetVoteById(int id)
        {
            if (id <= 0)
                return BadRequest("Id must be a positive integer.");

            var voteItem = await _voteRepository.GetVoteByIdAsync(id);

            if (voteItem == null)
                return NotFound();

            return Ok(voteItem);
        }

        [HttpPost("cast/{voteOptionId}")]
        public async Task<ActionResult> CastVote(int voteOptionId)
        {
            var voteOption = await _voteRepository.CastVoteAsync(voteOptionId);

            if (voteOption == null)
            {
                return NotFound("Vote option not found");
            }

            return Ok(new { voteOptionId = voteOption.Id, voteCount = voteOption.VoteCount });
        }
    }
}
