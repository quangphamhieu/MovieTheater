using MT.Dtos.Actor;
using MT.Dtos.Director;
using MT.Dtos.Genre;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.Movie
{
    public class CreateMovieDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên phim không được bỏ trống")]
        [MaxLength(255, ErrorMessage = "Tên phim không được vượt quá 255 ký tự")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mô tả không được bỏ trống")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Ngày phát hành là bắt buộc")]
        public DateTime ReleaseDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Thời lượng phải lớn hơn 0")]
        public int Duration { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Quốc gia không được bỏ trống")]
        public string Country { get; set; }

        public string PosterUrl { get; set; }

        [Required(ErrorMessage = "Đạo diễn là bắt buộc")]
        public DirectorDto Director { get; set; }

        public List<ActorDto> Actors { get; set; } = new List<ActorDto>();
        public List<GenreDto> Genres { get; set; } = new List<GenreDto>();
    }
}
