using MovieTheater.Dtos.Comment;

namespace MovieTheater.Service.Abstract
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDTO>> GetCommentsByMovieIdAsync(int movieId);
        Task<CommentDTO> GetCommentByIdAsync(int commentId);
        Task<CommentDTO> CreateCommentAsync(CreateCommentDto commentCreateDTO);
        Task<CommentDTO> UpdateCommentAsync(int commentId, UpdateCommentDto commentUpdateDTO);
        Task<bool> DeleteCommentAsync(int commentId);
    }
}
