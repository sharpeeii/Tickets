using Business.Interfaces.Auth;
using Common.Exceptions;
using Common.Helpers;
using Data.Interfaces;
using Data.Models.User;
using Data.Entities;
using Data.Repository;
using Microsoft.AspNetCore.Identity;

namespace Business.Services.Auth;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _accountRepo;
    private readonly IJwtService _jwtService;

    public AccountService(IAccountRepository accountRepo, IJwtService jwtService)
    {
        _accountRepo = accountRepo;
        _jwtService = jwtService;
    }

    public async Task RegisterAdmin(UserCreateModel model)
    {
        ValidationHelper.ValidateSignUpData(model.Username, model.Password, model.Email);
        
        if (await _accountRepo.EmailExistsAsync(model.Email))
        {
            throw new InvalidInputException("Admin with this email already exists!");
        }
        
        if (await _accountRepo.UsernameExistsAsync(model.Username))
        {
            throw new InvalidInputException("Admin with this username already exists!");
        }
        
        User newAdmin = new User()
        {
            Username = model.Username,
            Email = model.Email,
            UserId = Guid.NewGuid(),
            Role = "Admin"
        };
        string passHash = new PasswordHasher<User>()
            .HashPassword(newAdmin, model.Password);
        newAdmin.PasswordHash = passHash;
        await _accountRepo.CreateAccountAsync(newAdmin);

    }

    public async Task RegisterUser(UserCreateModel model)
    {
        ValidationHelper.ValidateSignUpData(model.Username, model.Password, model.Email);
        
        if (await _accountRepo.EmailExistsAsync(model.Email))
        {
            throw new InvalidInputException("User with this email already exists!");
        }

        if (await _accountRepo.UsernameExistsAsync(model.Username))
        {
            throw new InvalidInputException("User with this username already exists!");
        }
        
        User newUser = new User()
        {
            Username = model.Username,
            Email = model.Email,
            UserId = Guid.NewGuid(),
        };
        string passHash = new PasswordHasher<User>()
            .HashPassword(newUser, model.Password);
        newUser.PasswordHash = passHash;
        await _accountRepo.CreateAccountAsync(newUser);
    }
 
    public async Task<string> Login(string email, string password)
    {
        User? userAcc = await _accountRepo.GetByEmailAsync(email);
        if (userAcc == null)
        { 
            throw new InvalidLoginExecption("User with this email and password does not exist!"); 
        }
        
        PasswordVerificationResult result = new PasswordHasher<User>()
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