using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MT.Shared.Constant.Database;

namespace MT.Domain
{
    [Table(nameof(TicketSeat), Schema = DbSchema.MovieTheater)]
    public class TicketSeat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }

        [ForeignKey(nameof(Seat))]
        public int SeatId { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "Available";

        public Seat Seat { get; set; }
        public Ticket Ticket { get; set; }
    }
}
