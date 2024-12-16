using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MT.Shared.Constant.Database;

namespace MT.Domain
{
    [Table(nameof(Ticket), Schema = DbSchema.MovieTheater)]
    public class Ticket
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [ForeignKey(nameof(ShowTime))]
        public int ShowTimeId { get; set; }

        public decimal TotalPrice { get; set; }
        public DateTime BookingTime { get; set; }

        [Required]
        [MaxLength(50)]
        public string Status { get; set; } = "Còn hạn"; // Trạng thái mặc định là "Còn hạn"

        public User User { get; set; }
        public ShowTime ShowTime { get; set; }
        public String PaymentStatus { get; set; } = "chưa thanh toán";
        public ICollection<TicketSeat> TicketSeats { get; set; } = new List<TicketSeat>();
        public ICollection<TicketDiscount> TicketDiscounts { get; set; }
    }

}
