using MovieTheater.Dtos.Seat;

namespace MovieTheater.Dtos.Ticket
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
    }
}
