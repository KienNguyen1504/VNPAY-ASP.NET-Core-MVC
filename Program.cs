using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VnpayIntegration;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình đọc config từ appsettings.json và User Secrets
builder.Services.Configure<VnPayConfig>(builder.Configuration.GetSection("VnPay"));

var app = builder.Build();

// Endpoint kiểm tra ứng dụng đọc được cấu hình hay chưa
// Lưu ý: Tuyệt đối không log hoặc trả về trực tiếp giá trị thật của HashSecret
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
