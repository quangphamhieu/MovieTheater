namespace MovieTheater.Dtos.ShowTime
{
    public class ShowTimeDto
    {
        public int Id { get; set; }
        public string MovieTitle { get; set; } // Tên phim
        public string CinemaName { get; set; } // Tên rạp
        public string CinemaRoomName { get; set; } // Tên phòng chiếu
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
