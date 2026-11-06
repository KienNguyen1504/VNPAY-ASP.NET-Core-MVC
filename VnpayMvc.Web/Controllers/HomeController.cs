using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VnpayMvc.Web.Models;

namespace VnpayMvc.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Trang quy chuẩn giao diện: màu, chữ, nút, trạng thái, thông báo.
        /// ?demo=alerts để xem thử thông báo TempData sau khi chuyển trang.
        /// </summary>
        public IActionResult UiKit(string? demo)
        {
            if (demo == "alerts")
            {
                TempData["Success"] = "Đã tạo đơn DS-20261101-0001.";
                TempData["Warning"] = "Đơn hàng đang chờ VNPAY xác nhận.";
                return RedirectToAction(nameof(UiKit));
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
