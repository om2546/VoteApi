using VoteApi.Models;

namespace VoteApi.Repositories
{
    public interface IVoteRepository
    {
        Task<VoteItem> CreateVoteAsync(VoteItem voteItem);
        Task<(List<VoteItem> items, int totalItems)> GetVotesAsync(int page, int pageSize);
        Task<VoteItem?> GetVoteByIdAsync(int id);
        Task<VoteOption?> CastVoteAsync(int voteOptionId);
    }
}