namespace MovieTheater.Dtos.Ticket
{
    public class TicketCreateRequest
    {
        public int ShowTimeId { get; set; }
        public List<int> SeatIds { get; set; }
    }

}
