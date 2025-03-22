using Data.Models.Vote;

namespace Buisness.Interfaces;

public interface IVoteService
{
    public Task CreateVoteAsync(VoteModel model, Guid userId);
    public Task DeleteVoteAsync(Guid userId, Guid filmId);
}