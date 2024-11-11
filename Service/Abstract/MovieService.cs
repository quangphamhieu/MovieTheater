using Microsoft.EntityFrameworkCore;
using MovieTheater.DbContexts;
using MovieTheater.Dtos.Actor;
using MovieTheater.Dtos.Director;
using MovieTheater.Dtos.Genre;
using MovieTheater.Dtos.Movie;
using MovieTheater.Entities;
using MovieTheater.Service.Implement;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Service.Abstract
{
    public class MovieService : IMovieService
    {
        private readonly ApplicationDbContext _context;

        public MovieService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all movies with Director, Actors, and Genres
        public async Task<List<MovieDto>> GetAllAsync()
        {
            return await _context.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Select(m => new MovieDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    ReleaseDate = m.ReleaseDate,
                    Duration = m.Duration,
                    Country = m.Country,
                    PosterUrl = m.PosterUrl,
                    Director = m.Director != null ? new DirectorDto { Id = m.Director.Id, Name = m.Director.Name } : null,
                    Actors = m.MovieActors.Select(ma => new ActorDto { Id = ma.Actor.Id, Name = ma.Actor.Name }).ToList(),
                    Genres = m.MovieGenres.Select(mg => new GenreDto { Id = mg.Genre.Id, Name = mg.Genre.Name }).ToList()
                }).ToListAsync();
        }

        // Get movie by title with Director, Actors, and Genres
        public async Task<MovieDto> GetByTitleAsync(string title)
        {
            var movie = await _context.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.Title == title);

            if (movie == null) return null;

            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Duration = movie.Duration,
                Country = movie.Country,
                PosterUrl = movie.PosterUrl,
                Director = movie.Director != null ? new DirectorDto { Id = movie.Director.Id, Name = movie.Director.Name } : null,
                Actors = movie.MovieActors.Select(ma => new ActorDto { Id = ma.Actor.Id, Name = ma.Actor.Name }).ToList(),
                Genres = movie.MovieGenres.Select(mg => new GenreDto { Id = mg.Genre.Id, Name = mg.Genre.Name }).ToList()
            };
        }

        // Get movie by ID with Director, Actors, and Genres
        public async Task<MovieDto> GetByIdAsync(int id)
        {
            var movie = await _context.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return null;

            return new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                ReleaseDate = movie.ReleaseDate,
                Duration = movie.Duration,
                Country = movie.Country,
                PosterUrl = movie.PosterUrl,
                Director = movie.Director != null ? new DirectorDto { Id = movie.Director.Id, Name = movie.Director.Name } : null,
                Actors = movie.MovieActors.Select(ma => new ActorDto { Id = ma.Actor.Id, Name = ma.Actor.Name }).ToList(),
                Genres = movie.MovieGenres.Select(mg => new GenreDto { Id = mg.Genre.Id, Name = mg.Genre.Name }).ToList()
            };
        }

        // Create a new movie with associated Actors and Genres
        public async Task<MovieDto> CreateAsync(CreateMovieDto dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Description = dto.Description,
                PosterUrl = dto.PosterUrl,
                ReleaseDate = dto.ReleaseDate,
                Duration = dto.Duration,
                Country = dto.Country,
                DirectorId = dto.Director.Id
            };

            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            // Link actors and genres to the movie
            foreach (var actorDto in dto.Actors)
            {
                var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == actorDto.Name);
                if (actor != null)
                {
                    _context.MovieActors.Add(new MovieActor { MovieId = movie.Id, ActorId = actor.Id });
                }
            }

            foreach (var genreDto in dto.Genres)
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreDto.Name);
                if (genre != null)
                {
                    _context.MovieGenres.Add(new MovieGenre { MovieId = movie.Id, GenreId = genre.Id });
                }
            }

            await _context.SaveChangesAsync();
            return await GetByIdAsync(movie.Id);
        }

        // Update an existing movie with associated Actors and Genres
        public async Task<MovieDto> UpdateAsync(int id, UpdateMovieDto dto)
        {
            var movie = await _context.Movies
                .Include(m => m.MovieActors)
                .Include(m => m.MovieGenres)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null) return null;

            movie.Title = dto.Title;
            movie.Description = dto.Description;
            movie.ReleaseDate = dto.ReleaseDate;
            movie.Duration = dto.Duration;
            movie.Country = dto.Country;
            movie.DirectorId = dto.Director.Id;

            _context.MovieActors.RemoveRange(movie.MovieActors);
            _context.MovieGenres.RemoveRange(movie.MovieGenres);

            foreach (var actorDto in dto.Actors)
            {
                var actor = await _context.Actors.FirstOrDefaultAsync(a => a.Name == actorDto.Name);
                if (actor != null)
                {
                    _context.MovieActors.Add(new MovieActor { MovieId = movie.Id, ActorId = actor.Id });
                }
            }

            foreach (var genreDto in dto.Genres)
            {
                var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Name == genreDto.Name);
                if (genre != null)
                {
                    _context.MovieGenres.Add(new MovieGenre { MovieId = movie.Id, GenreId = genre.Id });
                }
            }

            await _context.SaveChangesAsync();
            return await GetByIdAsync(id);
        }

        // Delete a movie
        public async Task<bool> DeleteAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null) return false;

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
