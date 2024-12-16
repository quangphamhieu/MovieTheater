using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.SqlServer;
using MT.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MT.Shared.Constant.Database;
using MT.Applicationservices.Module.Abstracts;
using MT.Applicationservices.Module.Implements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MT.Applicationservices.Common;

namespace MT.Applicationservices.Startup
{
    public static class MovieTheaterStartup
    {
        public static void ConfigureMT(this WebApplicationBuilder builder, string? assemblyName)
        {
            builder.Services.AddDbContext<MovieTheaterDbContext>(
                options =>
                {
                    options.UseSqlServer(
                        builder.Configuration.GetConnectionString("Default"),
                        options =>
                        {
                            options.MigrationsAssembly(assemblyName);
                            options.MigrationsHistoryTable(
                                DbSchema.TableMigrationsHistory,
                                DbSchema.MovieTheater
                            );
                        }
                    );
                },
                ServiceLifetime.Scoped
            );

            builder.Services.AddScoped<IActorService, ActorService>();
            builder.Services.AddScoped<IDirectorService, DirectorService>();
            builder.Services.AddScoped<IGenreService, GenreService>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<ICinemaService, CinemaService>();
            builder.Services.AddScoped<ICinemaRoomService, CinemaRoomService>();
            builder.Services.AddScoped<IShowTimeService, ShowTimeService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ICommentService, CommentService>();
            builder.Services.AddScoped<ISeatService, SeatService>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<AuthenticationService>();
            builder.Services.AddScoped<EmailService>();
            builder.Services.AddScoped<IVietQrService, VietQrService>();
            builder.Services.AddScoped<IDiscountService, DiscountService>();
        }
    }
}
