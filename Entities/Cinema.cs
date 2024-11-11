namespace MovieTheater.Entities
{
    public class Cinema
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        // Navigation property
        public ICollection<CinemaRoom> CinemaRooms { get; set; }
    }
}
