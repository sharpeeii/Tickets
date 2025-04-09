using Business.Interfaces;
using Common.Exceptions;
using Common.Helpers;
using Data.DTOs.Vote;
using Data.Entities;
using Data.Interfaces;

namespace Business.Services;

public class VoteService : IVoteService
{
    private readonly IVoteRepository _voteRepo;
    private readonly IFilmRepository _filmRepo;
    private readonly IUnitOfWork _unit;
    private readonly IAccountRepository _accountRepo;

    public VoteService(IVoteRepository voteRepo, IFilmRepository filmRepo, 
        IUnitOfWork unit, IAccountRepository accountRepo)
    {
        _voteRepo = voteRepo;
        _filmRepo = filmRepo;
        _unit = unit;
        _accountRepo = accountRepo;
    }

    public async Task CreateVoteAsync(VoteDto dto, Guid userId)
    {
        ValidationHelper.ValidateRatingValue(dto.Rating);
        
        if (await _voteRepo.VoteExistsAsync(userId, dto.FilmId))
        {
            throw new EntityExistsException("Film already rated by this user!");
        }
        
        Film? film = await _filmRepo.GetFilmAsync(dto.FilmId);

        if (film == null)
        {
            throw new NotFoundException("Film not found!");
        }

        
        Vote newVote = new Vote()
        {
            VoteId = Guid.NewGuid(),
            FilmId = dto.FilmId,
            UserId = userId,
            Rating = dto.Rating
        };
        
        await _unit.BeginTransactionAsync();
        try
        {
            film.RatingSum += dto.Rating;
            film.RatingAmount++;
            film.Rating = (float)film.RatingSum / film.RatingAmount;
            
            await _voteRepo.CreateVoteAsync(newVote);
            
            await _unit.SaveChangesAsync();
            await _unit.CommitAsync();
        }
        catch (Exception)
        {
            await _unit.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteVoteAsync(Guid userId, Guid filmId)
    {
        Vote? vote = await _voteRepo.GetVoteAsync(userId, filmId);
        if (vote == null)
        {
            throw new NotFoundException("Vote not found!");
        }

        Film? film = await _filmRepo.GetFilmAsync(filmId);

        if (film == null)
        {
            throw new NotFoundException("Film not found!");
        }

        await _unit.BeginTransactionAsync();
        try
        {   
            film.RatingSum -= vote.Rating;
            film.RatingAmount--;
            if (film.RatingSum == 0 && film.RatingAmount == 0)
            {
                film.Rating = 0;
            }
            else
            {
                film.Rating = (float)film.RatingSum / film.RatingAmount;
            }
            
            await _voteRepo.DeleteVoteAsync(vote.VoteId);

            await _unit.SaveChangesAsync();
            await _unit.CommitAsync();
        }
        catch (Exception)
        {
            await _unit.RollbackAsync();
            throw;
        }
    }


} 