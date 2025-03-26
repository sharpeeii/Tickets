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
                x.UseNpgsql("Host=db;Port=5432;Username=postgres;Password=1234;Database=tickets-database");
            });
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