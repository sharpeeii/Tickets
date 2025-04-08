using Business.Interfaces.Auth;
using Business.Interfaces;
using Business.Services;
using Business.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class Extensions
    {
        public static IServiceCollection AddBuisnessLogic(this IServiceCollection services)
        {
            services.AddScoped<IHallService, HallService>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFilmService, FilmService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IVoteService, VoteService>();
            return services;
        }
    }
} 

//   ,-.       _,---._ __  / \
//  /  )    .-'       `./ /   \    <----- кладет классные зависимости в классный контейнер!!!
// (  (   ,'            `/    /|   
//  \  `-"             \'\   / |      
//   `.              ,  \ \ /  |
//    /`.          ,'-`----Y   |
//   (           ;         |   '
//   |  ,-.    ,-'         |  /
//   |  | (   |            | /
//   )  |  \  `.___________|/
//   `--'   `--'