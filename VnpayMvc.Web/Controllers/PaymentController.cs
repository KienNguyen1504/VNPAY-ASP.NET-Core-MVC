
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VnpayMvc.Web.Config;
using VnpayMvc.Web.Services;

namespace VnpayMvc.Web.Controllers
{
    public class PaymentController : Controller
    {
        private readonly VnPayConfig _config;

        public PaymentController(
            IOptions<VnPayConfig> config)
        {
            _config = config.Value;
        }

        [HttpGet("/Payment/Return")]
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Return()
        {
            bool validSignature =
                VnPaySignatureVerifier.IsValid(
                    Request.Query,
                    _config.HashSecret);

            if (!validSignature)
            {
                ViewBag.Message =
                    "Chữ ký VNPAY không hợp lệ.";
                return View();
            }

            ViewBag.Message =
                "Đã xác thực chữ ký VNPAY. " +
                "Đang chờ xác nhận giao dịch.";

            return View();
        }
    }
}
