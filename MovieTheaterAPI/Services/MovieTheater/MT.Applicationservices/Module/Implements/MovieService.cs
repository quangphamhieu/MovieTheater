using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.Actor;
using MT.Dtos.Director;
using MT.Dtos.Genre;
using MT.Dtos.Movie;
using MT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class MovieService : MovieTheaterBaseService, IMovieService
    {
        public MovieService(ILogger<MovieService> logger, MovieTheaterDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<List<MovieDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all movies.");
            return await _dbContext.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieGenres)
            .ThenInclude(mg => mg.Genre)
                .Select(m => MapToMovieDto(m))
                .ToListAsync();
        }

        public async Task<MovieDto> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Fetching movie with ID {id}.");
            var movie = await _dbContext.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                _logger.LogWarning($"Movie with ID {id} not found.");
                return null;
            }

            return MapToMovieDto(movie);
        }

        public async Task<MovieDto> GetByTitleAsync(string title)
        {
            _logger.LogInformation($"Fetching movie with title '{title}'.");
            var movie = await _dbContext.Movies
                .Include(m => m.Director)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync(m => m.Title == title);

            if (movie == null)
            {
                _logger.LogWarning($"Movie with title '{title}' not found.");
                return null;
            }

            return MapToMovieDto(movie);
        }

        public async Task<MovieDto> CreateAsync(CreateMovieDto dto)
        {
            _logger.LogInformation($"Creating new movie '{dto.Title}'.");
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

            _dbContext.Movies.Add(movie);
            await _dbContext.SaveChangesAsync();

            await LinkActorsAndGenresAsync(movie.Id, dto.Actors, dto.Genres);
            return await GetByIdAsync(movie.Id);
        }

        public async Task<MovieDto> UpdateAsync(int id, UpdateMovieDto dto)
        {
            _logger.LogInformation($"Updating movie with ID {id}.");
            var movie = await _dbContext.Movies
                .Include(m => m.MovieActors)
                .Include(m => m.MovieGenres)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie == null)
            {
                _logger.LogWarning($"Movie with ID {id} not found.");
                return null;
            }

            movie.Title = dto.Title;
            movie.Description = dto.Description;
            movie.ReleaseDate = dto.ReleaseDate;
            movie.Duration = dto.Duration;
            movie.Country = dto.Country;
            movie.DirectorId = dto.Director.Id;

            _dbContext.MovieActors.RemoveRange(movie.MovieActors);
            _dbContext.MovieGenres.RemoveRange(movie.MovieGenres);

            await LinkActorsAndGenresAsync(id, dto.Actors, dto.Genres);
            await _dbContext.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"Deleting movie with ID {id}.");
            var movie = await _dbContext.Movies.FindAsync(id);
            if (movie == null)
            {
                _logger.LogWarning($"Movie with ID {id} not found.");
                return false;
            }

            _dbContext.Movies.Remove(movie);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<MovieDto>> SearchMoviesByTitleAsync(string title)
        {
            _logger.LogInformation($"Searching movies containing '{title}'.");
            return await _dbContext.Movies
                .Where(m => m.Title.Contains(title))
                .Include(m => m.Director)
                .Include(m => m.MovieActors)
                    .ThenInclude(ma => ma.Actor)
                .Include(m => m.MovieGenres)
                    .ThenInclude(mg => mg.Genre)
                .Select(m => MapToMovieDto(m))
                .ToListAsync();
        }

        private static MovieDto MapToMovieDto(Movie movie)
        {
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

        private async Task LinkActorsAndGenresAsync(int movieId, List<ActorDto> actors, List<GenreDto> genres)
        {
            foreach (var actorDto in actors)
            {
                var actor = await _dbContext.Actors.FirstOrDefaultAsync(a => a.Name == actorDto.Name);
                if (actor != null)
                {
                    _dbContext.MovieActors.Add(new MovieActor { MovieId = movieId, ActorId = actor.Id });
                }
            }

            foreach (var genreDto in genres)
            {
                var genre = await _dbContext.Genres.FirstOrDefaultAsync(g => g.Name == genreDto.Name);
                if (genre != null)
                {
                    _dbContext.MovieGenres.Add(new MovieGenre { MovieId = movieId, GenreId = genre.Id });
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
