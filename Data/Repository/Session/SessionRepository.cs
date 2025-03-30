using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Common.Helpers;

    
namespace Data.Repository;

public class SessionRepository : ISessionRepository
{
    private readonly AppDbContext _context;

    public SessionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreateSessionAsync(SessionEntity session)
    {
        await _context.Sessions.AddAsync(session);
        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<SessionEntity>> GetAllSessionsAsync()
    {
        ICollection<SessionEntity> sessions = await _context.Sessions
            .AsNoTracking()
            .Include(s=>s.Film)
            .Include(s => s.Hall)
            .ToListAsync();
        return sessions;
    }

    public async Task<SessionEntity?> GetSessionAsync(Guid sessionId)
    {
        SessionEntity? session = await _context.Sessions
            .AsNoTracking()
            .Include(s=>s.Film)
            .Include(s => s.Hall)
            .FirstOrDefaultAsync(s => s.Id == sessionId);
        return session;
    }

    public async Task UpdateSessionHallAsync(Guid sessionId, Guid hallId)
    {
        SessionEntity? session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.Id == sessionId);
        if (session == null)
        {
            throw new NullReferenceException("Session does not exist!");
        }

        session.HallId = hallId;
        await _context.SaveChangesAsync();
    }

    public async Task UpdateSessionDateAsync(Guid sessionId, DateTime date)
    {
        SessionEntity? session = await _context.Sessions
            .FirstOrDefaultAsync(s => s.Id == sessionId);
        if (session == null)
        {
            throw new NullReferenceException("Session does not exist!");
        }

        session.SessionDate = date;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteSessionAsync(Guid sessionId)
    {
        await _context.Sessions
            .Where(s => s.Id == sessionId)
            .ExecuteDeleteAsync();
        await _context.SaveChangesAsync();
    }

    public async Task<bool> SessionDateExistsAsync(DateTime date, Guid hallId)
    {
        return await _context.Sessions
            .AnyAsync(s => s.SessionDate == date && s.HallId == hallId);
    }
    
}