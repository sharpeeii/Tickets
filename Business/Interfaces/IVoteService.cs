using Data.Models.Vote;

namespace Business.Interfaces;

public interface IVoteService
{
    public Task CreateVoteAsync(VoteModel model, Guid userId);
    public Task DeleteVoteAsync(Guid userId, Guid filmId);
}