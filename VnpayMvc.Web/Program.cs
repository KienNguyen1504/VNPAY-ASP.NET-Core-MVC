using VnpayMvc.Web.Config;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.Configure<VnPayConfig>(builder.Configuration.GetSection("VnPay"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
// API Kiểm thử nạp cấu hình VNPAY
app.MapGet("/api/test-config", (IOptions<VnPayConfig> configOptions) =>
{
    var config = configOptions.Value;
    return new 
    {
        Message = "Đọc cấu hình VNPAY thành công!",
        TmnCode_Loaded = !string.IsNullOrEmpty(config.TmnCode),
        HashSecret_Loaded = !string.IsNullOrEmpty(config.HashSecret),
        BaseUrl = config.BaseUrl,
        ReturnUrl = config.ReturnUrl
    };
});

app.Run();
