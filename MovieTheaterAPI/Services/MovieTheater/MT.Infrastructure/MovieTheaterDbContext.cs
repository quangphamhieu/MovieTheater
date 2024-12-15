using Microsoft.EntityFrameworkCore;
using MT.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Infrastructure
{
    public class MovieTheaterDbContext : DbContext
    {
        public MovieTheaterDbContext(DbContextOptions<MovieTheaterDbContext> options) : base(options) { }

        // DbSets for entities
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Director> Directors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieActor> MovieActors { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<CinemaRoom> CinemaRooms { get; set; }
        public DbSet<ShowTime> ShowTimes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketSeat> TicketSeats { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship for Movie and Actor
            modelBuilder.Entity<MovieActor>()
                .HasKey(ma => new { ma.MovieId, ma.ActorId });

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Movie)
                .WithMany(m => m.MovieActors)
                .HasForeignKey(ma => ma.MovieId);

            modelBuilder.Entity<MovieActor>()
                .HasOne(ma => ma.Actor)
                .WithMany(a => a.MovieActors)
                .HasForeignKey(ma => ma.ActorId);

            // Configure many-to-many relationship for Movie and Genre
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId);

            // Configure one-to-many relationship between Cinema and CinemaRoom
            modelBuilder.Entity<Cinema>()
               .HasMany(c => c.CinemaRooms)
               .WithOne(cr => cr.Cinema)
               .HasForeignKey(cr => cr.CinemaId)
               .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between CinemaRoom and ShowTime
            modelBuilder.Entity<CinemaRoom>()
                .HasMany(cr => cr.ShowTimes)
                .WithOne(st => st.CinemaRoom)
                .HasForeignKey(st => st.CinemaRoomId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Movie and ShowTime
            modelBuilder.Entity<ShowTime>()
                .HasOne(st => st.Movie)
                .WithMany(m => m.ShowTimes)
                .HasForeignKey(st => st.MovieId);

            // Cấu hình mối quan hệ giữa User và Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

            // Configure one-to-many relationship between User and Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Movie and Comment
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Movie)
                .WithMany(m => m.Comments)
                .HasForeignKey(c => c.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TicketSeat>()
                .HasOne(ts => ts.Ticket)
                .WithMany(t => t.TicketSeats)
                .HasForeignKey(ts => ts.TicketId)
                .OnDelete(DeleteBehavior.NoAction);  // Đặt hành động xóa là NoAction

            modelBuilder.Entity<TicketSeat>()
                .HasOne(ts => ts.Seat)
                .WithMany()
                .HasForeignKey(ts => ts.SeatId)
                .OnDelete(DeleteBehavior.NoAction);  // Đặt hành động xóa là NoAction cho Seat

            // Cấu hình xóa cho các bảng khác (nếu có)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<UserNotification>()
           .HasKey(un => new { un.UserId, un.NotificationId });

            modelBuilder.Entity<UserNotification>()
                .HasOne(un => un.User)
                .WithMany()
                .HasForeignKey(un => un.UserId);

            modelBuilder.Entity<UserNotification>()
                .HasOne(un => un.Notification)
                .WithMany()
                .HasForeignKey(un => un.NotificationId);



            var adminRole = new Role { Id = 1, RoleName = "Admin" };
            var customerRole = new Role { Id = 2, RoleName = "Customer" };

            modelBuilder.Entity<Role>().HasData(adminRole, customerRole);

            var adminUser = new User
            {
                Id = 1,
                FullName = "Admin User",
                Email = "admin@example.com",
                Password = "Admin@123",
                PhoneNumber = "0123456789",
                Address = "123 Admin Street",
                RoleId = adminRole.Id
            };

            modelBuilder.Entity<User>().HasData(adminUser);
        }


    }
}
