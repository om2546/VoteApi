using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VoteApi.Models;

namespace VoteApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoteController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VoteController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<VoteItem>> CreateVote([FromBody] VoteItem voteItem)
        {
            if (voteItem == null)
            {
                return BadRequest("Vote item cannot be null");
            }

            voteItem.CreatedAt = DateTime.UtcNow;

            foreach (var option in voteItem.VoteOptions)
            {
                option.VoteCount = 0;
            }

            _context.VoteItems.Add(voteItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(CreateVote), new { id = voteItem.Id }, voteItem);
        }

        [HttpGet]
        public async Task<ActionResult> GetVotes([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page < 1)
                page = 1;
            
            if (pageSize < 1)
                pageSize = 10;

            var totalItems = await _context.VoteItems.CountAsync();

            var voteItems = await _context.VoteItems
                .Include(v => v.VoteOptions)
                .OrderBy(v => v.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            
            return Ok(new 
            {
                items = voteItems,
                page,
                pageSize,
                totalItems,
                totalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            });
        }
    }
}
