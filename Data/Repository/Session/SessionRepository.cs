using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Common.Helpers;

    
namespace Data.Repository.Session;

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

    public async Task<ICollection<SessionEntity>> GetSessionsByFilmAsync(Guid filmId)
    {
        ICollection<SessionEntity> sessions = await _context.Sessions
            .AsNoTracking()
            .Where(s=> s.FilmId==filmId)
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
            .AnyAsync(s => s.StartDate == date && s.HallId == hallId);
    }
    
    public async Task<ICollection<SessionEntity>> GetAllSessionsAsync()
    {
        ICollection<SessionEntity> sessions = await _context.Sessions
            .AsNoTracking()
            .ToListAsync();
        return sessions;
    }

    public async Task<ICollection<SessionEntity>> GetSessionsByDayAsync(DateTime date)
    {
        ICollection<SessionEntity> sessions = await _context.Sessions
            .AsNoTracking()
            .Where(s => s.StartDate.Day == date.Day)
            .ToListAsync();
        return sessions;
    }
}