using System;

namespace MovieTheater.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; } 
        public User User { get; set; }
        public Movie Movie { get; set; }
    }
}
