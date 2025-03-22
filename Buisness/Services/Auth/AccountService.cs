using Buisness.Interfaces;
using Common.Exceptions;
using Common.Helpers;
using Data.Interfaces;
using Data.Models.User;
using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Identity;

namespace Buisness.Services.Auth;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepo;
    private readonly IJwtService _jwtService;

    public AccountService(IAccountRepository accountRepo, IJwtService jwtService)
    {
        _accountRepo = accountRepo;
        _jwtService = jwtService;
    }


    public async Task Register(UserCreateModel model)
    {
        ValidationHelper.ValidateSignUpData(model.Username, model.Password, model.Email);
        
        if (await _accountRepo.CheckIfEmailExists(model.Email))
        {
            throw new InvalidInputException("User with this email already exists!");
        }

        if (await _accountRepo.CheckIfUsernameExists(model.Username))
        {
            throw new InvalidInputException("User with this username already exists!");
        }
        
        UserEntity newUser = new UserEntity()
        {
            Username = model.Username,
            Email = model.Email,
            Id = Guid.NewGuid(),
        };
        string passHash = new PasswordHasher<UserEntity>()
            .HashPassword(newUser, model.Password);
        newUser.PasswordHash = passHash;
        await _accountRepo.CreateAccountAsync(newUser);
    }
 
    public async Task<string> Login(string email, string password)
    {
        UserEntity? userAcc = await _accountRepo.GetByEmailAsync(email);
        if (userAcc == null)
        { 
            throw new InvalidLoginExecption("User with this email and password does not exist!"); 
        }
        
        PasswordVerificationResult result = new PasswordHasher<UserEntity>()
            .VerifyHashedPassword(userAcc, userAcc.PasswordHash, password);
        
        if (result == PasswordVerificationResult.Success)
        {
            return _jwtService.GenerateToken(userAcc);
        }
        else
        {
            throw new InvalidLoginExecption("User with this email and password does not exist!"); 
        }
    }
}