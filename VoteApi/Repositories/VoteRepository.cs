using Microsoft.EntityFrameworkCore;
using VoteApi.Models;

namespace VoteApi.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private readonly AppDbContext _context;

        public VoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<VoteItem> CreateVoteAsync(VoteItem voteItem)
        {
            voteItem.CreatedAt = DateTime.UtcNow;

            foreach (var option in voteItem.VoteOptions)
            {
                option.VoteCount = 0;
            }

            _context.VoteItems.Add(voteItem);
            await _context.SaveChangesAsync();

            return voteItem;
        }

        public async Task<(List<VoteItem> items, int totalItems)> GetVotesAsync(int page, int pageSize)
        {
            var totalItems = await _context.VoteItems.CountAsync();

            var voteItems = await _context.VoteItems
                .Include(v => v.VoteOptions)
                .OrderBy(v => v.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (voteItems, totalItems);
        }

        public async Task<VoteItem?> GetVoteByIdAsync(int id)
        {
            return await _context.VoteItems
                .Include(v => v.VoteOptions)
                .AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<VoteOption?> CastVoteAsync(int voteOptionId)
        {
            var voteOption = await _context.VoteOptions.FindAsync(voteOptionId);

            if (voteOption == null)
            {
                return null;
            }

            voteOption.VoteCount += 1;
            await _context.SaveChangesAsync();

            return voteOption;
        }
    }
}