using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Data.Interfaces;
using Data.Repository;
using Data.Repository.Session;
using Data.Repository.Film;
using Data.Repository.Seat;

namespace Data
{
    public static class Extensions
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services)
        {   
            services.AddScoped<IHallRepository, HallRepository>();
            services.AddScoped<SeatRepository>();
            services.AddScoped<ISeatRepository, CachedSeatRepository>();
            services.AddScoped<ISeatTypeRepository, SeatTypeRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<FilmRepository>();
            services.AddScoped<IFilmRepository, CachedFilmRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IVoteRepository, VoteRepository>();
            services.AddScoped<DbSeeder>();

            
            services.AddStackExchangeRedisCache(options =>
                options.Configuration = "redis_service:6379");
            
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