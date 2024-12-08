using MT.Dtos.Comment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Abstracts
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
