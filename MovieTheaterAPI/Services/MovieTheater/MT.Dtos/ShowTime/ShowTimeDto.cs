using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.ShowTime
{
    public class ShowTimeDto
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; }
        public string CinemaName { get; set; }
        public string CinemaRoomName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
