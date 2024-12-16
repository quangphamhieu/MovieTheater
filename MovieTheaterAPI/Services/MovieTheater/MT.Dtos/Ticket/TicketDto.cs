using MT.Dtos.Seat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Dtos.Ticket
{
    public class TicketDto
    {
        public int TicketId { get; set; }
        public string MovieTitle { get; set; }
        public string CinemaName { get; set; }
        public string RoomName { get; set; }
        public DateTime ShowTime { get; set; }
        public List<SeatDto> Seats { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingTime { get; set; }
        public string Status { get; set; } // Thêm trạng thái vé
        public string DiscountCode { get; set; }

    }

}
