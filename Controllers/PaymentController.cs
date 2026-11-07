
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VnPayMvc.Web.Config;
using VnPayMvc.Web.Data;
using VnPayMvc.Web.Services;

namespace VnPayMvc.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly VnPayConfig _config;

        public PaymentController(
            ApplicationDbContext db,
            IOptions<VnPayConfig> config)
        {
            _db = db;
            _config = config.Value;
        }

        [HttpGet("/Payment/Return")]
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public async Task<IActionResult> Return()
        {
            // 1. Kiểm tra chữ ký VNPAY
            if (!VnPaySignatureVerifier.IsValid(
                    Request.Query, _config.HashSecret))
            {
                return Content("Chu ky VNPAY khong hop le.");
            }

            // 2. Đọc thông tin giao dịch
            string txnRef = Request.Query["vnp_TxnRef"].ToString();
            string amountText = Request.Query["vnp_Amount"].ToString();

            if (string.IsNullOrWhiteSpace(txnRef) ||
                !long.TryParse(amountText, out long amount) ||
                amount <= 0)
            {
                return Content("Thong tin giao dich khong hop le.");
            }

            // 3. Tìm Payment và Order trong database
            var payment = await _db.Payments
                .AsNoTracking()
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.TxnRef == txnRef);

            if (payment == null || payment.Order == null)
            {
                return Content("Khong tim thay giao dich.");
            }

            // 4. Đối chiếu số tiền (VNPAY nhân 100)
            if (payment.Amount * 100m != amount ||
                payment.Order.TotalAmount != payment.Amount)
            {
                return Content("So tien giao dich khong khop.");
            }

            // 5. Chỉ đọc trạng thái, KHÔNG cập nhật database
            if (payment.Status ==
                VnPayMvc.Web.Models.Enums.PaymentStatus.Pending)
            {
                return Content(
                    "Da tiep nhan ket qua VNPAY. " +
                    "Giao dich dang cho IPN xac nhan.");
            }

            return Content(
                $"Trang thai thanh toan hien tai: {payment.Status}");
        }
    }
}
