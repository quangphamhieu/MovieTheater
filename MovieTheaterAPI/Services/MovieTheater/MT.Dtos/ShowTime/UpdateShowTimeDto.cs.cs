using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.ShowTime
{
    public class UpdateShowTimeDto
    {
        [Required]
        public string MovieTitle { get; set; }

        [Required]
        public string CinemaName { get; set; }

        [Required]
        public string CinemaRoomName { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}
