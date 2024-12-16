using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Infrastructure;
using QRCoder;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Implements
{
    public class VietQrService : IVietQrService
    {
        private readonly MovieTheaterDbContext _dbContext;
        private readonly EmailService _emailService;
        private readonly ILogger<VietQrService> _logger;
        private readonly HttpClient _httpClient;

        public VietQrService(MovieTheaterDbContext dbContext, EmailService emailService, ILogger<VietQrService> logger, HttpClient httpClient)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _logger = logger;
            _httpClient = httpClient;
        }

        // Sinh QR thanh toán
         public async Task<string> GeneratePaymentQrAsync(int ticketId)
        {
            var ticket = await _dbContext.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            if (ticket == null) throw new Exception("Ticket not found.");

            string bankCode = "TCB"; // Techcombank
            string accountNumber = "19071642735018"; // Số tài khoản nhận
            string accountName = "Phạm Quang Hiếu"; // Tên chủ tài khoản
            decimal amount = ticket.TotalPrice;
            string addInfo = $"Thanh toán vé {ticketId}";

            string qrUrl = $"https://img.vietqr.io/image/{bankCode}-{accountNumber}-compact.jpg?amount={amount}&addInfo={Uri.EscapeDataString(addInfo)}";

            return qrUrl;
        }

        // Sinh QR thông tin vé

        // Gửi email với thông tin vé và QR thông tin vé
        public async Task SendPaymentSuccessEmailAsync(int ticketId)
        {
            var ticket = await _dbContext.Tickets
                .Include(t => t.ShowTime)
                    .ThenInclude(s => s.Movie)
                .Include(t => t.ShowTime)
                    .ThenInclude(s => s.CinemaRoom)
                        .ThenInclude(cr => cr.Cinema)
                .Include(t => t.TicketSeats)
                    .ThenInclude(ts => ts.Seat)
                .FirstOrDefaultAsync(t => t.Id == ticketId);

            if (ticket == null) throw new Exception("Ticket not found.");

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == ticket.UserId);
            if (user == null) throw new Exception("User not found.");

            // Tạo QR chứa thông tin vé

            string subject = "Xác nhận thanh toán vé thành công";
            string message = $@"
        <div style='font-family: Arial, sans-serif; line-height: 1.6;'>
            <h2 style='color: #4CAF50;'>Chào {user.FullName},</h2>
            <p>Bạn đã thanh toán thành công vé xem phim với mã vé <strong>{ticket.Id}</strong>.</p>
            <h3>Thông tin vé:</h3>
            <ul>
                <li><strong>Phim:</strong> {ticket.ShowTime.Movie.Title}</li>
                <li><strong>Rạp:</strong> {ticket.ShowTime.CinemaRoom.Cinema.Name}</li>
                <li><strong>Phòng chiếu:</strong> {ticket.ShowTime.CinemaRoom.Name}</li>
                <li><strong>Thời gian:</strong> {ticket.ShowTime.StartTime:yyyy-MM-dd HH:mm} - {ticket.ShowTime.EndTime:HH:mm}</li>
                <li><strong>Ghế:</strong> {string.Join(", ", ticket.TicketSeats.Select(ts => ts.Seat.SeatCode))}</li>
                <li><strong>Tổng tiền:</strong> {ticket.TotalPrice:N0} VND</li>
            </ul>
            <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
        </div>";

            await _emailService.SendEmailAsync(user.Email, subject, message);
        }


        
    }
}
