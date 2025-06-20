// File: App.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using clasProduct;
using ProducDbContext;

public class App
{
    private readonly ProducDbContext.ProducDbContext _context;

    public App(ProducDbContext.ProducDbContext context) => _context = context;

    public async Task RunAsync()
    {
        await EnsureDatabaseCreatedAsync();
        PrintMenu();

        var choice = Console.ReadLine()?.Trim().ToLower();
        if (string.IsNullOrWhiteSpace(choice))
        {
            Console.WriteLine("❌ Bạn chưa nhập lựa chọn nào.");
            return;
        }

        if (choice == "huy") return;

        switch (choice)
        {
            case "1": await HandleInsertAsync(); break;
            case "2": await ReadProductAsync(); break;
            case "3": await HandleUpdateAsync(); break;
            case "4": await HandleRemoveAsync(); break;
            default: await PromptRetryAsync(); break;
        }
    }

    private async Task EnsureDatabaseCreatedAsync()
    {
        var created = await _context.Database.EnsureCreatedAsync();
        if (created)
            Console.WriteLine("✅ Cơ sở dữ liệu đã được tạo thành công!");
    }

    private void PrintMenu()
    {
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("1. Nhập sản phẩm vào CSDL");
        Console.WriteLine("2. Hiển thị danh sách sản phẩm");
        Console.WriteLine("3. Cập nhật sản phẩm");
        Console.WriteLine("4. Xóa sản phẩm");
        Console.WriteLine("Nhập 'huy' để thoát");
        Console.WriteLine("--------------------------------------------------");
        Console.Write("Lựa chọn của bạn: ");
    }

    private async Task PromptRetryAsync()
    {
        Console.WriteLine("Lựa chọn không hợp lệ.");
        await RunAsync();
    }

    private async Task HandleInsertAsync()
    {
        Console.WriteLine("Bạn đã chọn nhập sản phẩm.");
        await InsertProductAsync();
        await ReadProductAsync();
    }

    private async Task HandleUpdateAsync()
    {
        Console.WriteLine("Bạn đã chọn cập nhật sản phẩm.");
        await UpdateProductAsync();
        await ReadProductAsync();
    }

    private async Task HandleRemoveAsync()
    {
        Console.WriteLine("Bạn đã chọn xóa sản phẩm.");
        await ReMoveProductAsync();
        await ReadProductAsync();
    }

    private async Task InsertProductAsync()
    {
        var products = new List<Product>();
        while (true)
        {
            var product = new Product();
            Console.Write("Tên sản phẩm: ");
            product.Name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(product.Name)) continue;

            Console.Write("Nhà cung cấp: ");
            product.Provider = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(product.Provider)) continue;

            products.Add(product);

            Console.Write("Tiếp tục? Nhập 'xong' để dừng: ");
            if (Console.ReadLine()?.Trim().ToLower() == "xong") break;
        }

        await _context.Products.AddRangeAsync(products);
        await _context.SaveChangesAsync();
        Console.WriteLine($"✅ Đã lưu {products.Count} sản phẩm!");
    }

    private async Task ReadProductAsync()
    {
        var products = await _context.Products.ToListAsync();
        if (products.Count == 0)
        {
            Console.WriteLine("❌ Không có sản phẩm nào.");
            return;
        }
        products.ForEach(p => p.Display());
    }

    private async Task UpdateProductAsync()
    {
        Console.Write("ID sản phẩm cần cập nhật: ");
        if (!int.TryParse(Console.ReadLine(), out var id) || id <= 0)
        {
            Console.WriteLine("❌ ID không hợp lệ.");
            return;
        }

          // Dùng LINQ để tìm sản phẩm theo ID
        var product = await _context.Products
                                    .Where(p => p.ProductId == id)
                                    .FirstOrDefaultAsync(); // hoặc SingleOrDefaultAsync()
        if (product == null)
        {
            Console.WriteLine("❌ Không tìm thấy sản phẩm.");
            return;
        }

        Console.Write("Tên mới: ");
        var name = Console.ReadLine();
        Console.Write("Nhà cung cấp mới: ");
        var provider = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(provider))
        {
            Console.WriteLine("❌ Tên hoặc nhà cung cấp không được để trống.");
            return;
        }

        product.Name = name;
        product.Provider = provider;
        await _context.SaveChangesAsync();
        Console.WriteLine("✅ Cập nhật thành công!");
    }

    private async Task ReMoveProductAsync()
    {
        Console.Write("ID sản phẩm cần xóa: ");
        if (!int.TryParse(Console.ReadLine(), out var id) || id <= 0)
        {
            Console.WriteLine("❌ ID không hợp lệ.");
            return;
        }

        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            Console.WriteLine("❌ Không tìm thấy sản phẩm.");
            return;
        }

        Console.Write("Xác nhận xóa? (y/n): ");
        if (Console.ReadLine()?.Trim().ToLower() != "y") return;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        Console.WriteLine("✅ Xóa thành công!");
    }
}
