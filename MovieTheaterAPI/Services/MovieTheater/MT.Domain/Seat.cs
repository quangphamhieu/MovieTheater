using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MT.Shared.Constant.Database;

namespace MT.Domain
{
    [Table(nameof(Seat), Schema = DbSchema.MovieTheater)]
    public class Seat
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(CinemaRoom))]
        public int CinemaRoomId { get; set; }

        [MaxLength(20)]
        public string SeatCode { get; set; }

        [MaxLength(20)]
        public string SeatType { get; set; } = "Regular";

        public decimal Price { get; set; }

        public CinemaRoom CinemaRoom { get; set; }
    }
}
