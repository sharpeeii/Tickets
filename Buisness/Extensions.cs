using Buisness.Interfaces;
using Buisness.Services;
using Buisness.Services.Auth;
using Microsoft.Extensions.DependencyInjection;

namespace Buisness
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
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IVoteService, VoteService>();
            return services;
        }
    }
} 

