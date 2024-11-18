namespace MovieTheater.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ShowTimeId { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime BookingTime { get; set; }

        // Navigation properties
        public User User { get; set; }
        public ShowTime ShowTime { get; set; }
        public ICollection<TicketSeat> TicketSeats { get; set; }
    }

}
