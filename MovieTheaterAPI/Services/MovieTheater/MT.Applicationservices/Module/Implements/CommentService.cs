using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Common;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.Comment;
using MT.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class CommentService : MovieTheaterBaseService, ICommentService
    {
        public CommentService(ILogger<CommentService> logger, MovieTheaterDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public async Task<IEnumerable<CommentDTO>> GetCommentsByMovieIdAsync(int movieId)
        {
            _logger.LogInformation($"Fetching comments for movie with ID {movieId}.");
            return await _dbContext.Comments
                .Where(c => c.MovieId == movieId)
                .Select(c => new CommentDTO
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    MovieId = c.MovieId,
                    Content = c.Content,
                    Rating = c.Rating,
                    CreatedAt = c.CreatedAt
                })
            .ToListAsync();
        }

        public async Task<CommentDTO> GetCommentByIdAsync(int commentId)
        {
            _logger.LogInformation($"Fetching comment with ID {commentId}.");
            var comment = await _dbContext.Comments.FindAsync(commentId);
            if (comment == null)
            {
                _logger.LogWarning($"Comment with ID {commentId} not found.");
                return null;
            }

            return new CommentDTO
            {
                Id = comment.Id,
                UserId = comment.UserId,
                MovieId = comment.MovieId,
                Content = comment.Content,
                Rating = comment.Rating,
                CreatedAt = comment.CreatedAt
            };
        }

        public async Task<CommentDTO> CreateCommentAsync(CreateCommentDto commentCreateDTO)
        {
            _logger.LogInformation($"Creating a new comment for movie with ID {commentCreateDTO.MovieId} by user {commentCreateDTO.UserId}.");

            var comment = new Comment
            {
                UserId = commentCreateDTO.UserId,
                MovieId = commentCreateDTO.MovieId,
                Content = commentCreateDTO.Content,
                Rating = commentCreateDTO.Rating,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Comment created successfully with ID {comment.Id}.");
            return new CommentDTO
            {
                Id = comment.Id,
                UserId = comment.UserId,
                MovieId = comment.MovieId,
                Content = comment.Content,
                Rating = comment.Rating,
                CreatedAt = comment.CreatedAt
            };
        }

        public async Task<CommentDTO> UpdateCommentAsync(int commentId, UpdateCommentDto commentUpdateDTO)
        {
            _logger.LogInformation($"Updating comment with ID {commentId}.");
            var comment = await _dbContext.Comments.FindAsync(commentId);

            if (comment == null)
            {
                _logger.LogWarning($"Comment with ID {commentId} not found.");
                return null;
            }

            comment.Content = commentUpdateDTO.Content;
            comment.Rating = commentUpdateDTO.Rating;

            _dbContext.Comments.Update(comment);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Comment with ID {commentId} updated successfully.");
            return new CommentDTO
            {
                Id = comment.Id,
                UserId = comment.UserId,
                MovieId = comment.MovieId,
                Content = comment.Content,
                Rating = comment.Rating,
                CreatedAt = comment.CreatedAt
            };
        }

        public async Task<bool> DeleteCommentAsync(int commentId)
        {
            _logger.LogInformation($"Deleting comment with ID {commentId}.");
            var comment = await _dbContext.Comments.FindAsync(commentId);

            if (comment == null)
            {
                _logger.LogWarning($"Comment with ID {commentId} not found.");
                return false;
            }

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Comment with ID {commentId} deleted successfully.");
            return true;
        }
    }
}
