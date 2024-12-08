using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MT.Shared.Constant.Database;

namespace MT.Domain
{
    [Table(nameof(Comment), Schema = DbSchema.MovieTheater)]
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [ForeignKey(nameof(Movie))]
        public int MovieId { get; set; }

        [MaxLength(500)]
        public string Content { get; set; }

        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public Movie Movie { get; set; }
    }
}
