using System.Collections.Generic;

namespace MovieTheater.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Ticket> Tickets { get; set; }

    }
}
