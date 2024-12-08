using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MT.Shared.Constant.Database;

namespace MT.Domain
{
    [Table(nameof(UserNotification), Schema = DbSchema.MovieTheater)]
    public class UserNotification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [ForeignKey(nameof(Notification))]
        public int NotificationId { get; set; }

        public bool IsRead { get; set; } = false;
        public DateTime DateReceived { get; set; } = DateTime.Now;

        public Notification Notification { get; set; }
        public User User { get; set; }
    }
}
