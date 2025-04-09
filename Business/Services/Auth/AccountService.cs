using Business.Interfaces.Auth;
using Common.Exceptions;
using Common.Helpers;
using Data.Interfaces;
using Data.DTOs.User;
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

    public async Task RegisterAdmin(UserCreateDto dto)
    {
        ValidationHelper.ValidateSignUpData(dto.Username, dto.Password, dto.Email);
        
        if (await _accountRepo.EmailExistsAsync(dto.Email))
        {
            throw new InvalidInputException("Admin with this email already exists!");
        }
        
        if (await _accountRepo.UsernameExistsAsync(dto.Username))
        {
            throw new InvalidInputException("Admin with this username already exists!");
        }
        
        User newAdmin = new User()
        {
            Username = dto.Username,
            Email = dto.Email,
            UserId = Guid.NewGuid(),
            Role = "Admin"
        };
        string passHash = new PasswordHasher<User>()
            .HashPassword(newAdmin, dto.Password);
        newAdmin.PasswordHash = passHash;
        await _accountRepo.CreateAccountAsync(newAdmin);

    }

    public async Task RegisterUser(UserCreateDto dto)
    {
        ValidationHelper.ValidateSignUpData(dto.Username, dto.Password, dto.Email);
        
        if (await _accountRepo.EmailExistsAsync(dto.Email))
        {
            throw new InvalidInputException("User with this email already exists!");
        }

        if (await _accountRepo.UsernameExistsAsync(dto.Username))
        {
            throw new InvalidInputException("User with this username already exists!");
        }
        
        User newUser = new User()
        {
            Username = dto.Username,
            Email = dto.Email,
            UserId = Guid.NewGuid(),
        };
        string passHash = new PasswordHasher<User>()
            .HashPassword(newUser, dto.Password);
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