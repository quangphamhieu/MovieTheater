namespace MovieTheater.Dtos.Comment
{
    public class CreateCommentDto
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
    }
}
