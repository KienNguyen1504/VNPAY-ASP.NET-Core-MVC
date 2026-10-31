# VNPAY Payment Gateway Integration – ASP.NET Core MVC

Dự án tích hợp cổng thanh toán trực tuyến **VNPAY** vào website thương mại điện tử được xây dựng trên nền tảng **ASP.NET Core MVC**.

Đây là dự án học phần **Phát triển ứng dụng Thương mại điện tử**, tập trung vào việc xây dựng quy trình đặt hàng, khởi tạo giao dịch thanh toán, xử lý kết quả giao dịch và đồng bộ trạng thái thanh toán giữa website và hệ thống VNPAY.

---

## 1. Giới thiệu dự án

### Mục tiêu

- Xây dựng website thương mại điện tử với chức năng xem sản phẩm và đặt hàng.
- Tích hợp VNPAY Sandbox để thực hiện thanh toán trực tuyến.
- Xây dựng quy trình khởi tạo giao dịch và chuyển hướng người dùng đến cổng thanh toán VNPAY.
- Xử lý kết quả thanh toán thông qua Return URL và IPN URL.
- Kiểm tra chữ ký bảo mật, mã đơn hàng, số tiền và trạng thái giao dịch.
- Đảm bảo tính nhất quán dữ liệu và hạn chế xử lý giao dịch trùng lặp.

### Phạm vi chức năng dự kiến

| Nhóm chức năng | Nội dung |
|---|---|
| Quản lý sản phẩm | Hiển thị danh sách và thông tin sản phẩm |
| Giỏ hàng | Thêm, cập nhật và quản lý sản phẩm trong giỏ hàng |
| Đặt hàng | Tạo đơn hàng và tính toán tổng giá trị đơn hàng |
| Thanh toán VNPAY | Khởi tạo giao dịch và tạo URL thanh toán |
| Xử lý kết quả | Tiếp nhận kết quả qua Return URL và IPN |
| Xác thực giao dịch | Kiểm tra chữ ký, số tiền, mã giao dịch và trạng thái |
| Quản lý thanh toán | Cập nhật và lưu trạng thái thanh toán vào cơ sở dữ liệu |

---

## 2. Công nghệ sử dụng

| Công nghệ | Vai trò |
|---|---|
| C# | Ngôn ngữ lập trình chính |
| ASP.NET Core MVC | Framework phát triển ứng dụng web |
| Entity Framework Core | ORM và tương tác với cơ sở dữ liệu |
| SQL Server / LocalDB | Hệ quản trị cơ sở dữ liệu |
| Razor Views | Xây dựng giao diện người dùng |
| HTML, CSS, JavaScript | Phát triển giao diện website |
| Bootstrap | Thiết kế giao diện responsive |
| VNPAY Sandbox | Môi trường thử nghiệm thanh toán |
| Git & GitHub | Quản lý mã nguồn và cộng tác nhóm |
| Visual Studio | Môi trường phát triển |

---

## 3. Cấu trúc dự án

```text
VNPAY-ASP.NET-Core-MVC/
│
├── VnpayMvc.Web/
│   │
│   ├── Controllers/
│   │   └── Xử lý các request và điều hướng nghiệp vụ
│   │
│   ├── Models/
│   │   └── Các entity và model của ứng dụng
│   │
│   ├── Views/
│   │   └── Các giao diện Razor
│   │
│   ├── Data/
│   │   └── DbContext và cấu hình dữ liệu
│   │
│   ├── Services/
│   │   └── Xử lý nghiệp vụ và tích hợp VNPAY
│   │
│   ├── ViewModels/
│   │   └── Các model phục vụ hiển thị dữ liệu
│   │
│   ├── wwwroot/
│   │   └── CSS, JavaScript, hình ảnh và thư viện giao diện
│   │
│   ├── Program.cs
│   │   └── Cấu hình và khởi chạy ứng dụng
│   │
│   └── appsettings.json
│       └── Cấu hình ứng dụng
│
├── VnpayMvc.Web.slnx
│   └── Solution của dự án
│
├── .gitignore
│
└── README.md
```

*Lưu ý: Một số thư mục trong cấu trúc trên được định hướng để phát triển và hoàn thiện trong quá trình thực hiện dự án.*

---

## 4. Cài đặt và chạy dự án

### Yêu cầu môi trường

- Visual Studio hỗ trợ ASP.NET Core.
- .NET SDK tương ứng với phiên bản TargetFramework trong file project.
- SQL Server hoặc SQL Server LocalDB.
- Git.
- Tài khoản VNPAY Sandbox để kiểm thử thanh toán.

### Bước 1. Clone repository

```bash
git clone https://github.com/KienNguyen1504/VNPAY-ASP.NET-Core-MVC.git
```

### Bước 2. Mở solution

Mở file:

```text
VnpayMvc.Web.slnx
```

bằng Visual Studio.

### Bước 3. Khôi phục các package

Mở Terminal tại thư mục chứa solution và chạy:

```bash
dotnet restore
```

### Bước 4. Cấu hình cơ sở dữ liệu

Cấu hình connection string phù hợp với SQL Server hoặc LocalDB trên máy cá nhân.

Không sử dụng connection string chứa thông tin nhạy cảm trong các commit công khai.

### Bước 5. Cấu hình VNPAY Sandbox

Thiết lập các thông số tích hợp VNPAY trong cấu hình cục bộ:

- `TmnCode`: Mã website/merchant được VNPAY cấp.
- `HashSecret`: Khóa bí mật dùng để tạo và xác thực chữ ký.
- `BaseUrl`: Địa chỉ cổng thanh toán Sandbox.
- `ReturnUrl`: Địa chỉ nhận kết quả thanh toán và chuyển hướng người dùng.
- `IpnUrl`: Địa chỉ tiếp nhận thông báo kết quả giao dịch từ VNPAY.

**Lưu ý bảo mật:** Không commit `HashSecret`, thông tin tài khoản hoặc các thông tin xác thực thực tế lên GitHub. Nên sử dụng User Secrets hoặc biến môi trường để lưu trữ thông tin nhạy cảm.

### Bước 6. Build và chạy ứng dụng

```bash
dotnet build
dotnet run
```

Hoặc nhấn `F5` trong Visual Studio để chạy ứng dụng.

---

## 5. Quy trình thanh toán VNPAY dự kiến

```text
Người dùng
    |
    v
Chọn sản phẩm và đặt hàng
    |
    v
Website tạo đơn hàng
    |
    v
Tính toán tổng tiền phía Server
    |
    v
Khởi tạo giao dịch VNPAY
    |
    v
Tạo URL thanh toán và chữ ký
    |
    v
Chuyển hướng đến VNPAY
    |
    v
Người dùng thực hiện thanh toán
    |
    v
VNPAY gửi kết quả giao dịch
    |
    +----------------------+
    |                      |
    v                      v
 Return URL              IPN URL
    |                      |
    v                      v
Hiển thị kết quả      Xác thực giao dịch
                           |
                           v
                    Cập nhật trạng thái
                    thanh toán vào DB
```

Hệ thống cần xác thực chữ ký và thông tin giao dịch ở phía Server trước khi cập nhật trạng thái thanh toán. Việc xử lý IPN cần đảm bảo tính idempotent để tránh cập nhật giao dịch nhiều lần.

---

## 6. Quy ước làm việc nhóm với Git

Để đảm bảo quá trình phát triển thống nhất, nhóm sử dụng GitHub để quản lý mã nguồn.

### Các nhánh làm việc

- `main`: Nhánh chính, lưu trữ phiên bản mã nguồn ổn định.
- `feature/...`: Nhánh phát triển chức năng.
- `fix/...`: Nhánh sửa lỗi.

Ví dụ:

```bash
git checkout -b feature/vnpay-payment
```

### Quy trình làm việc

1. Pull mã nguồn mới nhất từ nhánh `main`.
2. Tạo branch riêng cho chức năng được phân công.
3. Phát triển và kiểm thử trên branch cá nhân.
4. Commit với nội dung mô tả rõ ràng.
5. Push branch lên GitHub.
6. Tạo Pull Request để đề xuất merge vào `main`.
7. Kiểm tra và xác nhận trước khi merge.

### Một số quy ước commit

```text
feat: add product listing
feat: integrate VNPAY payment
fix: validate payment signature
fix: handle duplicate IPN callback
docs: update project README
refactor: separate payment service
```

Không làm việc trực tiếp trên `main` khi phát triển chức năng, trừ các trường hợp được nhóm thống nhất.

---

## 7. Thành viên thực hiện

| STT | Họ tên |
|---|---|
| 1 | Kiên |
| 2 | Phương |
| 3 | Thắng |
| 4 | Tuệ |
| 5 | Nhi |
| 6 | Hưng |
| 7 | Hoa |

---

## 8. Tài liệu tham khảo

- [VNPAY – Cổng thanh toán điện tử](https://vnpay.vn/)
- [VNPAY Developer Documentation](https://sandbox.vnpayment.vn/apis/)
- [Microsoft – ASP.NET Core Documentation](https://learn.microsoft.com/aspnet/core/)
- [Microsoft – Entity Framework Core](https://learn.microsoft.com/ef/core/)
- [GitHub Documentation](https://docs.github.com/)

---

## 9. Thông tin repository

**GitHub:** https://github.com/KienNguyen1504/VNPAY-ASP.NET-Core-MVC

**Project:** VNPAY Payment Gateway Integration – ASP.NET Core MVC

**Course:** Phát triển ứng dụng Thương mại điện tử

**Status:** In Development

---

*This project is developed for academic purposes as part of the E-commerce Application Development course.*
