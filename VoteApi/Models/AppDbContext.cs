using Microsoft.EntityFrameworkCore;

namespace VoteApi.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<VoteItem> VoteItems { get; set; } = null!;
        public DbSet<VoteOption> VoteOptions { get; set; } = null!;
    
    }
}
