using Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using Data.Repository;

namespace Data
{
    public static class Extensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {   
            services.AddScoped<IHallRepository, HallRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IFilmRepository, FilmRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IVoteRepository, VoteRepository>();

            services.AddDbContext<AppDbContext>(x => 
            {
                x.UseNpgsql("Host=localhost;Database=cinema;Username=postgres;Password=qwerty123");
            });
            return services;
        }
    }
}


//   ,-.       _,---._ __  / \
//  /  )    .-'       `./ /   \
// (  (   ,'            `/    /|   
//  \  `-"             \'\   / |      
//   `.              ,  \ \ /  |
//    /`.          ,'-`----Y   |
//   (           ;         |   '
//   |  ,-.    ,-'         |  /
//   |  | (   |            | /
//   )  |  \  `.___________|/
//   `--'   `--'