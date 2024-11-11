using MovieTheater.Dtos.Actor;
using MovieTheater.Dtos.Director;
using MovieTheater.Dtos.Genre;

namespace MovieTheater.Dtos.Movie
{
    public class UpdateMovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string Country { get; set; }
        public string PosterUrl { get; set; }

        public DirectorDto Director { get; set; }
        public List<ActorDto> Actors { get; set; } = new List<ActorDto>();
        public List<GenreDto> Genres { get; set; } = new List<GenreDto>();
    }
}
