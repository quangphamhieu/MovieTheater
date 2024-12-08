using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MT.Applicationservices.Module.Abstracts;
using MT.Domain;
using MT.Dtos.Payment;
using MT.Infrastructure;
using Newtonsoft.Json;
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
        private readonly HttpClient _httpClient;
        private readonly MovieTheaterDbContext _dbContext;
        private readonly EmailService _emailService;
        private readonly ILogger<VietQrService> _logger;  // Khai báo _logger

        public VietQrService(HttpClient httpClient, MovieTheaterDbContext dbContext, EmailService emailService, ILogger<VietQrService> logger)
        {
            _httpClient = httpClient;
            _dbContext = dbContext;
            _emailService = emailService;
            _logger = logger;  // Khởi tạo _logger
        }

        // Lấy Access Token từ OAuth2
        public async Task<string> GetAccessTokenAsync()
        {
            string clientId = "your_client_id";
            string clientSecret = "your_client_secret";
            string tokenUrl = "https://api.vietqr.io/oauth2/token";

            var requestBody = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret)
            });

            var response = await _httpClient.PostAsync(tokenUrl, requestBody);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

            return tokenResponse.AccessToken;
        }

        // Tạo URL QR thanh toán
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

        // Kiểm tra giao dịch từ API
        public async Task<bool> VerifyPaymentAsync(int ticketId)
        {
            _logger.LogInformation("Verifying payment for ticketId: " + ticketId);  // Ghi log thông tin ticketId

            string accessToken = await GetAccessTokenAsync();
            string accountNumber = "19071642735018"; // Số tài khoản nhận

            var requestUrl = $"https://api.vietqr.io/v2/transactions?account={accountNumber}";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.GetAsync(requestUrl);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed to get transactions. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
                return false;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var transactionResponse = JsonConvert.DeserializeObject<TransactionResponse>(responseContent);

            var ticket = await _dbContext.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId);
            if (ticket == null)
            {
                _logger.LogWarning($"Ticket with Id {ticketId} not found.");
                return false;
            }

            var transaction = transactionResponse.Transactions.FirstOrDefault(t =>
                t.AddInfo.Contains($"Thanh toán vé {ticketId}") && t.Amount == ticket.TotalPrice && t.Status == "SUCCESS");

            if (transaction == null)
            {
                _logger.LogWarning($"No successful transaction found for ticketId {ticketId}.");
                return false;
            }

            // Cập nhật trạng thái thanh toán
            ticket.Status = "Đã thanh toán";
            _dbContext.Tickets.Update(ticket);

            var payment = new Payment
            {
                TicketId = ticketId,
                Amount = ticket.TotalPrice,
                Status = "Đã thanh toán",
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = "VietQR",
                TransactionId = transaction.TransactionId
            };

            await _dbContext.Payments.AddAsync(payment);
            await _dbContext.SaveChangesAsync();

            // Gửi email thông báo
            await SendPaymentSuccessEmail(ticket);

            return true;
        }

        // Gửi email thông báo sau khi thanh toán thành công
        private async Task SendPaymentSuccessEmail(Ticket ticket)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == ticket.UserId);
            if (user == null) throw new Exception("User not found.");

            string subject = "Xác nhận thanh toán vé thành công";
            string message = $@"
                <h1>Chào {user.FullName},</h1>
                <p>Bạn đã thanh toán thành công vé xem phim với mã vé <strong>{ticket.Id}</strong>.</p>
                <p>Thông tin chi tiết:</p>
                <ul>
                    <li>Phim: {ticket.ShowTime.Movie.Title}</li>
                    <li>Rạp: {ticket.ShowTime.CinemaRoom.Cinema.Name}</li>
                    <li>Thời gian: {ticket.ShowTime}</li>
                    <li>Số tiền: {ticket.TotalPrice} VND</li>
                </ul>
                <p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi!</p>
            ";

            await _emailService.SendEmailAsync(user.Email, subject, message);
        }

        // Chuẩn hóa chuỗi: loại bỏ dấu trong tiếng Việt
        private static string RemoveDiacritics(string input)
        {
            string normalizedString = input.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
