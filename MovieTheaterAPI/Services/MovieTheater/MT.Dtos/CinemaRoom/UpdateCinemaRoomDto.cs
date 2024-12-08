using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.CinemaRoom
{
    public class UpdateCinemaRoomDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên phòng không được bỏ trống")]
        public string Name { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số hàng ghế phải là số hợp lệ và lớn hơn 0")]
        public int SeatRows { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Số cột ghế phải là số hợp lệ và lớn hơn 0")]
        public int SeatColumns { get; set; }
    }
}
