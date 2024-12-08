using MT.Dtos.Actor;
using MT.Dtos.Director;
using MT.Dtos.Genre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.Movie
{
    public class MovieDto
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
