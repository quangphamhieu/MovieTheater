using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MT.Shared.Constant.Database;

namespace MT.Domain
{
    [Table(nameof(User), Schema = DbSchema.MovieTheater)]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(150)]
        public string Email { get; set; }

        [MaxLength(255)]
        public string Password { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }

        [MaxLength(300)]
        public string Address { get; set; }

        public bool EmailConfirmed { get; set; } = false; // Trạng thái xác thực email

        public string? EmailConfirmToken { get; set; } // Mã token để xác thực email

        public string? ResetPasswordToken { get; set; } // Token đặt lại mật khẩu

        [ForeignKey(nameof(Role))]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public ICollection<UserDiscount> UserDiscounts { get; set; }
    }
}
