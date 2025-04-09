using Data.Entities;

namespace Data.Interfaces;

public interface IVoteRepository
{
    public Task CreateVoteAsync(Vote vote);
    public Task DeleteVoteAsync(Guid voteId);
    public Task<bool> VoteExistsAsync(Guid userId, Guid filmId);
    public Task<Vote> GetVoteAsync(Guid userId, Guid filmId);
}