using System.Net.Sockets;

namespace MovieTheater.Entities
{
    public class TicketSeat
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int SeatId { get; set; }
        public string Status { get; set; } = "Available";

        public Seat Seat { get; set; }
        public Ticket Ticket { get; set; }
    }
}
