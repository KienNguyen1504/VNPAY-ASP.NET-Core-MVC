
using Microsoft.AspNetCore.Mvc;

namespace VnPayMvc.Web.Controllers
{
    public class PaymentController : Controller
    {
        [HttpGet("/Payment/Return")]
        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Return()
        {
            // Chưa xác nhận thanh toán từ IPN.
            return Content(
                "Da tiep nhan Return URL. " +
                "Giao dich dang cho xac nhan.");
        }
    }
}
