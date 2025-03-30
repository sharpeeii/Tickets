using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using Data.Repository;
using Data.Repository.Film;

namespace Data
{
    public static class Extensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {   
            services.AddScoped<IHallRepository, HallRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<FilmRepository>();
            services.AddScoped<IFilmRepository, CachedFilmRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IVoteRepository, VoteRepository>();

            
            services.AddMemoryCache(); 
            services.AddDbContext<AppDbContext>(x => 
            {
                x.UseNpgsql("Host=db;Port=5432;Username=postgres;Password=1234;Database=tickets-database");
            });
            services.AddStackExchangeRedisCache(options =>
                options.Configuration = "cacher:6379");
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