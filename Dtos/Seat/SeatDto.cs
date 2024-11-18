namespace MovieTheater.Dtos.Seat
{
    public class SeatDto
    {
        public int Id { get; set; }
        public string SeatCode { get; set; }
        public string SeatType { get; set; }
        public decimal Price { get; set; }

        public SeatDto() { }

        public SeatDto(MovieTheater.Entities.Seat seat)
        {
            Id = seat.Id;
            SeatCode = seat.SeatCode;
            SeatType = seat.SeatType;
            Price = seat.Price;
        }
    }
}
