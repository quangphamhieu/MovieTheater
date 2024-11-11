namespace MovieTheater.Dtos.CinemaRoom
{
    public class CreateCinemaRoomDto
    {
        public string Name { get; set; }
        public string CinemaName { get; set; }
        public int SeatRows { get; set; }
        public int SeatColumns { get; set; }
    }
}
