using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Abstracts
{
    public interface IVietQrService
    {
        Task<string> GeneratePaymentQrAsync(int ticketId); // Sinh QR thanh toán
        Task<string> GenerateTicketQrAsync(int ticketId);  // Sinh QR thông tin vé
        Task SendPaymentSuccessEmailAsync(int ticketId);   // Gửi email xác nhận
    }
}
