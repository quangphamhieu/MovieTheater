using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MT.Shared.Constant.Database;

namespace MT.Domain
{
    [Table(nameof(Movie), Schema = DbSchema.MovieTheater)]
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(200)]
        public string Title { get; set; }

        public string Description { get; set; }

        [MaxLength(500)]
        public string PosterUrl { get; set; }

        public DateTime ReleaseDate { get; set; }
        public int Duration { get; set; }

        [MaxLength(100)]
        public string Country { get; set; }

        [ForeignKey(nameof(Director))]
        public int DirectorId { get; set; }
        public Director Director { get; set; }

        public ICollection<MovieActor> MovieActors { get; set; } = new List<MovieActor>();
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
        public ICollection<ShowTime> ShowTimes { get; set; } = new List<ShowTime>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
