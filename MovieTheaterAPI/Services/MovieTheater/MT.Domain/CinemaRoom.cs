using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MT.Shared.Constant.Database;

namespace MT.Domain
{
    [Table(nameof(CinemaRoom), Schema = DbSchema.MovieTheater)]
    public class CinemaRoom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey(nameof(Cinema))]
        public int CinemaId { get; set; }

        public Cinema Cinema { get; set; }

        public int SeatRows { get; set; }
        public int SeatColumns { get; set; }

        public ICollection<ShowTime> ShowTimes { get; set; } = new List<ShowTime>();
    }
}
