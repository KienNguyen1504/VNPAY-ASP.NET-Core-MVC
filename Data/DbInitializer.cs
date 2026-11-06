using System;
using System.Linq;
using VnPayMvc.Web.Models;

namespace VnPayMvc.Web.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return; // Đã có dữ liệu
            }

            var products = new Product[]
            {
                new Product
                {
                    Name = "Áo Thun Nam Basic",
                    Description = "Áo thun chất liệu cotton 100% thoáng mát, co giãn 4 chiều.",
                    Price = 150000,
                    ImageUrl = "/images/ao-thun.jpg",
                    StockQuantity = 50,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "Quần Jean Slim Fit",
                    Description = "Quần jean nam dáng ôm vừa vặn, màu xanh đen lịch sự.",
                    Price = 350000,
                    ImageUrl = "/images/quan-jean.jpg",
                    StockQuantity = 30,
                    CreatedAt = DateTime.Now
                },
                new Product
                {
                    Name = "Giày Sneaker Thể Thao",
                    Description = "Giày thể thao nam êm chân, phù hợp đi chơi và tập luyện.",
                    Price = 500000,
                    ImageUrl = "/images/sneaker.jpg",
                    StockQuantity = 20,
                    CreatedAt = DateTime.Now
                }
            };

            context.Products.AddRange(products);
            context.SaveChanges();
        }
    }
}

