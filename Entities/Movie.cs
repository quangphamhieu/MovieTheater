using System;
using System.Collections.Generic;

namespace MovieTheater.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }
        public string Country { get; set; }
        public int DirectorId { get; set; }
        public Director Director { get; set; }
        public ICollection<MovieActor> MovieActors { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; }
        public ICollection<ShowTime> ShowTimes { get; set; }
    }
}
