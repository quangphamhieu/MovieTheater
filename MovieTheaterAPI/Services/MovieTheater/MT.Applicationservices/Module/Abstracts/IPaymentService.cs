using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Applicationservices.Module.Abstracts
{
    public interface IVietQrService
    {
        Task<string> GetAccessTokenAsync();
        Task<string> GeneratePaymentQrAsync(int ticketId);
        Task<bool> VerifyPaymentAsync(int ticketId);
    }
}
