
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VnPayMvc.Web.Config;
using VnPayMvc.Web.Controllers;
using VnPayMvc.Web.Data;
using VnPayMvc.Web.Models;
using VnPayMvc.Web.Models.Enums;
using Xunit;

namespace VnPayReturn.Tests;

public class PaymentControllerTests
{
    private const string Secret = "FAKE_SECRET_FOR_TEST_ONLY";

    [Theory]
    [InlineData("ORDER001", "15000000",
        "Đã tiếp nhận kết quả VNPAY")]
    [InlineData("UNKNOWN", "15000000",
        "Không tìm thấy giao dịch")]
    [InlineData("ORDER001", "99999999",
        "Số tiền giao dịch không khớp")]
    public async Task Return_ShouldValidateTransaction(
        string txnRef, string amount, string expectedMessage)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var db = new ApplicationDbContext(options);

        var order = new Order
        {
            Id = 1,
            ProductId = 1,
            OrderCode = "ORDER001",
            TotalAmount = 150000m,
            Status = OrderStatus.Pending
        };

        var payment = new Payment
        {
            Id = 1,
            OrderId = 1,
            TxnRef = "ORDER001",
            Amount = 150000m,
            Status = PaymentStatus.Pending
        };

        db.Orders.Add(order);
        db.Payments.Add(payment);
        await db.SaveChangesAsync();

        var config = Options.Create(new VnPayConfig
        {
            HashSecret = Secret
        });

        var controller = new PaymentController(db, config);

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var query = CreateSignedQuery(txnRef, amount);
        controller.HttpContext.Request.QueryString =
            QueryString.Create(query);

        var result = await controller.Return();

        Assert.IsType<ViewResult>(result);
        Assert.Contains(
            expectedMessage,
            controller.ViewData["Message"]?.ToString());

        // Return URL không được tự đánh dấu đã thanh toán
        var savedPayment = await db.Payments.FindAsync(1);
        Assert.Equal(PaymentStatus.Pending, savedPayment!.Status);
    }

    private static Dictionary<string, string> CreateSignedQuery(
        string txnRef, string amount)
    {
        var parameters = new SortedDictionary<string, string>
        {
            ["vnp_Amount"] = amount,
            ["vnp_ResponseCode"] = "00",
            ["vnp_TxnRef"] = txnRef
        };

        string signData = string.Join("&",
            parameters.Select(p =>
                WebUtility.UrlEncode(p.Key) + "=" +
                WebUtility.UrlEncode(p.Value)));

        using var hmac = new HMACSHA512(
            Encoding.UTF8.GetBytes(Secret));

        string signature = Convert.ToHexString(
            hmac.ComputeHash(Encoding.UTF8.GetBytes(signData)));

        var result = parameters.ToDictionary(
            p => p.Key, p => p.Value);

        result["vnp_SecureHash"] = signature;
        return result;
    }
}
