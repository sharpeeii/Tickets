using Data.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace Data.Interfaces;

public interface IVoteRepository
{
    public Task CreateVoteAsync(VoteEntity vote);
    public Task DeleteVoteAsync(Guid voteId);
    public Task<bool> VoteExistsAsync(Guid userId, Guid filmId);
    public Task<VoteEntity> GetVoteAsync(Guid userId, Guid filmId);
}