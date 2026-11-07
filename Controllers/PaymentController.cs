
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
        ViewData["Message"] = "Chữ ký VNPAY không hợp lệ.";
        return View("Return");
    }

    // 2. Đọc thông tin giao dịch
    string txnRef = Request.Query["vnp_TxnRef"].ToString();
    string amountText = Request.Query["vnp_Amount"].ToString();

    if (string.IsNullOrWhiteSpace(txnRef) ||
        !long.TryParse(amountText, out long amount) ||
        amount <= 0)
    {
        ViewData["Message"] = "Thông tin giao dịch không hợp lệ.";
        return View("Return");
    }

    // 3. Tìm Payment và Order trong database
    var payment = await _db.Payments
        .AsNoTracking()
        .Include(p => p.Order)
        .FirstOrDefaultAsync(p => p.TxnRef == txnRef);

    if (payment == null || payment.Order == null)
    {
        ViewData["Message"] = "Không tìm thấy giao dịch.";
        return View("Return");
    }

    // 4. Đối chiếu số tiền
    if (payment.Amount * 100m != amount ||
        payment.Order.TotalAmount != payment.Amount)
    {
        ViewData["Message"] = "Số tiền giao dịch không khớp.";
        return View("Return");
    }

    // 5. Chỉ hiển thị trạng thái từ database
    if (payment.Status ==
        VnPayMvc.Web.Models.Enums.PaymentStatus.Pending)
    {
        ViewData["Message"] =
            "Đã tiếp nhận kết quả VNPAY. " +
            "Giao dịch đang chờ IPN xác nhận.";

        return View("Return");
    }

    ViewData["Message"] =
        $"Trạng thái thanh toán hiện tại: {payment.Status}";

    return View("Return");
}

    }
}
