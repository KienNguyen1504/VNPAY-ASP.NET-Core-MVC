
using System.Net;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using VnPayMvc.Web.Services;
using Xunit;

namespace VnPayReturn.Tests;

public class VnPaySignatureVerifierTests
{
    private const string TestSecret = "TEST_SECRET_ONLY_NOT_REAL";

    [Fact]
    public void ValidSignature_ShouldBeAccepted()
    {
        var query = CreateSignedQuery(
            "ORDER001", "15000000");

        bool result = VnPaySignatureVerifier.IsValid(
            query, TestSecret);

        Assert.True(result);
    }

    [Fact]
    public void InvalidSignature_ShouldBeRejected()
    {
        var query = CreateSignedQuery(
            "ORDER001", "15000000");

        query["vnp_Amount"] = "99999999";

        bool result = VnPaySignatureVerifier.IsValid(
            new QueryCollection(query), TestSecret);

        Assert.False(result);
    }

    [Fact]
    public void MissingSignature_ShouldBeRejected()
    {
        var query = new QueryCollection(
            new Dictionary<string,
                Microsoft.Extensions.Primitives.StringValues>
            {
                ["vnp_TxnRef"] = "ORDER001",
                ["vnp_Amount"] = "15000000"
            });

        bool result = VnPaySignatureVerifier.IsValid(
            query, TestSecret);

        Assert.False(result);
    }

    private static Dictionary<string,
        Microsoft.Extensions.Primitives.StringValues>
        CreateSignedQuery(string txnRef, string amount)
    {
        var parameters = new SortedDictionary<string, string>
        {
            ["vnp_Amount"] = amount,
            ["vnp_TxnRef"] = txnRef,
            ["vnp_ResponseCode"] = "00"
        };

        string signData = string.Join("&",
            parameters.Select(p =>
                WebUtility.UrlEncode(p.Key) + "=" +
                WebUtility.UrlEncode(p.Value)));

        using var hmac = new HMACSHA512(
            Encoding.UTF8.GetBytes(TestSecret));

        string signature = Convert.ToHexString(
            hmac.ComputeHash(
                Encoding.UTF8.GetBytes(signData)));

        var result = parameters.ToDictionary(
            p => p.Key,
            p => (Microsoft.Extensions.Primitives.StringValues)p.Value);

        result["vnp_SecureHash"] = signature;

        return result;
    }
}
