namespace MovieTheater.Entities
{
    public class Seat
    {
        public int Id { get; set; }
        public int CinemaRoomId { get; set; }
        public string SeatCode { get; set; }
        public string SeatType { get; set; } = "Regular";
        public decimal Price { get; set; }

        public CinemaRoom CinemaRoom { get; set; }
    }
}
