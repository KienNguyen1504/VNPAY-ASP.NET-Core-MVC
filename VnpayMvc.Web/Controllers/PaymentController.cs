
using Microsoft.AspNetCore.Mvc;

namespace VnpayMvc.Web.Controllers
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
            // Chua xac nhan thanh toan.
            // Se bo sung kiem tra chu ky va don hang sau.
         return View();
        }
    }
}
