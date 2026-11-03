using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VnPayMvc.Web.Data;

namespace VnpayMvc.Web.Controllers
{
    /// <summary>
    /// Trang danh sách sản phẩm — lấy dữ liệu từ DB, không hard-code.
    /// Nút "Mua ngay" gọi OrderController.CreateOrder của Phương.
    /// </summary>
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Where(p => p.StockQuantity > 0)
                .OrderBy(p => p.Id)
                .ToListAsync();

            return View(products);
        }
    }
}
