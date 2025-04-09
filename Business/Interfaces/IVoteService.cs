using Data.DTOs.Vote;

namespace Business.Interfaces;

public interface IVoteService
{
    public Task CreateVoteAsync(VoteDto dto, Guid userId);
    public Task DeleteVoteAsync(Guid userId, Guid filmId);
}