using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class VoteRepository : IVoteRepository
{
    private readonly AppDbContext _context;

    public VoteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateVoteAsync(VoteEntity vote)
    {
        await _context.Votes.AddAsync(vote);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteVoteAsync(Guid voteId)
    {
        await _context.Votes
            .Where(v=> v.Id == voteId)
            .ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    public async Task<bool> VoteExistsAsync(Guid userId, Guid filmId)
    {
        return await _context.Votes.AnyAsync(v =>  v.UserId == userId&& v.FilmId == filmId);
    }

    public async Task<VoteEntity> GetVoteAsync(Guid userId, Guid filmId)
    {
        VoteEntity? vote = await _context.Votes
            .Where(v => v.UserId == userId && v.FilmId == filmId)
            .FirstOrDefaultAsync();
        return vote;
    }
}