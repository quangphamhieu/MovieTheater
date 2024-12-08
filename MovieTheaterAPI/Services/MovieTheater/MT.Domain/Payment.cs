using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MT.Shared.Constant.Database;

namespace MT.Domain
{
    [Table(nameof(Payment), Schema = DbSchema.MovieTheater)]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }  // ID thanh toán (chỉ định là long)

        // Đặt khóa ngoại cho Ticket
        [ForeignKey(nameof(Ticket))]
        public int TicketId { get; set; }

        // Các thuộc tính khác của Payment
        public decimal Amount { get; set; }  // Số tiền thanh toán
        public string Status { get; set; }  // Trạng thái thanh toán (Chưa thanh toán, Đã thanh toán)
        public DateTime PaymentDate { get; set; }  // Thời gian thanh toán
        public string PaymentMethod { get; set; }  // Phương thức thanh toán
        public string TransactionId { get; set; }  // Mã giao dịch

        // Mối quan hệ với Ticket (một Payment có một Ticket)
        public Ticket Ticket { get; set; }

        
    }
}
