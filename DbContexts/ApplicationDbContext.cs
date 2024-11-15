﻿using Microsoft.EntityFrameworkCore;
using MovieTheater.Entities;

namespace MovieTheater.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

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
