using Business.Interfaces;
using Common.Exceptions;
using Common.Helpers;
using Data.Entities;
using Data.Interfaces;
using Data.Models.Vote;

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

    public async Task CreateVoteAsync(VoteModel model, Guid userId)
    {
        ValidationHelper.ValidateRatingValue(model.Rating);
        
        if (await _voteRepo.VoteExistsAsync(userId, model.FilmId))
        {
            throw new EntityExistsException("Film already rated by this user!");
        }
        
        FilmEntity? film = await _filmRepo.GetFilmAsync(model.FilmId);

        if (film == null)
        {
            throw new NotFoundException("Film not found!");
        }

        
        VoteEntity newVote = new VoteEntity()
        {
            Id = Guid.NewGuid(),
            FilmId = model.FilmId,
            UserId = userId,
            Rating = model.Rating
        };
        
        await _unit.BeginTransactionAsync();
        try
        {
            film.RatingSum += model.Rating;
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
        VoteEntity? vote = await _voteRepo.GetVoteAsync(userId, filmId);
        if (vote == null)
        {
            throw new NotFoundException("Vote not found!");
        }

        FilmEntity? film = await _filmRepo.GetFilmAsync(filmId);

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
            
            await _voteRepo.DeleteVoteAsync(vote.Id);

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