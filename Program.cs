using Microsoft.EntityFrameworkCore;
using VnPayMvc.Web.Data;
using VnPayMvc.Web.Config;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký ApplicationDbContext kết nối SQL Server LocalDB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();
builder.Services.Configure<VnPayConfig>(
    builder.Configuration.GetSection("VnPay"));

var app = builder.Build();

// Tự động Seed dữ liệu sản phẩm mẫu khi ứng dụng khởi chạy
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    DbInitializer.Seed(context);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
