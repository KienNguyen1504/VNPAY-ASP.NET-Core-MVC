# VnPay Payment Service (Chương 3, mục 8)

Tạo URL thanh toán VNPAY (Sandbox, API 2.1.0) từ một Order đã có trong database:
dựng bộ tham số, quy đổi amount x100, sắp xếp tham số, ký HMAC-SHA512.

## Cấu trúc
- src/PaymentService/Services/VnPayService.cs   : service chính (CreatePaymentUrl)
- src/PaymentService/Helpers/VnPayLibrary.cs    : sắp xếp, URL-encode, HMAC-SHA512, VerifySignature
- src/PaymentService/Options/VnPayOptions.cs    : class cấu hình (section "VnPay")
- src/PaymentService/Models/                    : Order/OrderStatus bản rút gọn (thay bằng model thật của nhóm)
- demo/VnPayDemo                                : in ra URL mẫu đã che thông tin
- demo/VnPayWebDemo                             : web MVC demo (mục 8): trang chi tiết đơn + nút Thanh toán + redirect Sandbox
- tests/PaymentService.Tests                    : unit test (vector kiểm chứng độc lập)

## Chạy
    dotnet test
    dotnet run --project demo/VnPayDemo

## Cấu hình secret (KHÔNG commit)
    cd <thư mục project web>
    dotnet user-secrets init
    dotnet user-secrets set "VnPay:TmnCode"    "<TmnCode VNPAY cấp>"
    dotnet user-secrets set "VnPay:HashSecret" "<HashSecret VNPAY cấp>"
Hoặc biến môi trường: VnPay__TmnCode, VnPay__HashSecret.
Các giá trị không bí mật (BaseUrl, ReturnUrl...) xem appsettings.sample.json.

## Ghép vào project MVC chung
1. Copy thư mục Services, Helpers, Options vào project (đổi namespace cho khớp).
2. Xóa Models/Order.cs, Models/OrderStatus.cs bản rút gọn; dùng model thật (cần OrderCode, TotalAmount, Status).
3. Program.cs:  builder.Services.AddVnPay(builder.Configuration);
4. Controller (mục 8 của Kiên):
       var order = await _db.Orders.FirstAsync(o => o.Id == id);   // đọc từ DB
       var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
       return Redirect(_vnPay.CreatePaymentUrl(order, ip));

## Demo luồng redirect (mục 8)
    cd demo/VnPayWebDemo
    dotnet user-secrets set "VnPay:TmnCode"    "<TmnCode>"
    dotnet user-secrets set "VnPay:HashSecret" "<HashSecret>"
    dotnet run --launch-profile https
Mở https://localhost:7001/Orders -> Chi tiết -> "Thanh toán qua VNPAY" -> trình duyệt chuyển sang trang Sandbox.
Luồng: Orders/Details (POST orderId + antiforgery) -> PaymentController.Pay -> đọc Order từ DB ->
VnPayService.CreatePaymentUrl -> Redirect(302). Payment/VnPayReturn hiện chỉ là stub (mục 9 xử lý thật).
