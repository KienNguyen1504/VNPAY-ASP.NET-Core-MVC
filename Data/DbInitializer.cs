using Microsoft.EntityFrameworkCore;

namespace VnPayMvc.Web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Tự động tạo cơ sở dữ liệu SQL Server LocalDB nếu chưa tồn tại
                context.Database.EnsureCreated();
            }
        }
    }
}
