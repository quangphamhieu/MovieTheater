using System.Net.Sockets;

namespace MovieTheater.Entities
{
    public class ShowTime
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int CinemaRoomId { get; set; }
        public CinemaRoom CinemaRoom { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
