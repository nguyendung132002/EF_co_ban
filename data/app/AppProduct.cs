using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using clasProduct;
using Microsoft.EntityFrameworkCore;
using ProducDbContext;

public class App
{
    private readonly ProducDbContext.ProducDbContext _context;

    public App(ProducDbContext.ProducDbContext context)
    {
        _context = context;
    }

    public async Task RunAsync()
    {
        await CreateDatabaseAsync();
        Console.WriteLine("--------------------------------------------------");
        Console.WriteLine("Bắt đầu nhập sản phẩm vào cơ sở dữ liệu vui lòng chọn 1:");  
        Console.WriteLine("Hiển thị danh sách sản phẩm lòng chọn 2:");
        Console.WriteLine("Câp nhật danh sách sản phẩm lòng chọn 3:");
        Console.WriteLine("xóa sản phẩm lòng chọn 4:");
        Console.WriteLine("Nếu bạn muốn hủy bỏ, hãy nhập 'huy' để thoát.");
        Console.WriteLine("--------------------------------------------------");
        Console.Write("Nhập lựa chọn của bạn (1 , 2 , 3 , 4 , 'huy'): ");
        var choice = Console.ReadLine();
        if(string.IsNullOrWhiteSpace(choice))
        {
            Console.WriteLine("❌  Bạn chưa nhập lựa chọn nào. Vui lòng nhập 1 , 2 , 3 , huy.");
            return;
        }
         if (choice?.Trim().ToLower() == "huy") return;
        switch (choice?.Trim())
        {
            case "1":
                Console.WriteLine("Bạn đã chọn nhập sản phẩm.");
                await InsertProductAsync();
                await ReadProductAsync();
                break;
            case "2":
                Console.WriteLine("Bạn đã chọn hiển thị danh sách sản phẩm.");
                await ReadProductAsync();
                break;
            case "3":
                Console.WriteLine("Bạn đã chọn cập nhật danh sách sản phẩm.");
                await UpdateProductAsync();
                Console.WriteLine(" danh sách sản phẩm đã cập nhật là:.");
                await ReadProductAsync();
                break;
                case "4":
                Console.WriteLine("Bạn đã chọn xóa sản phẩm.");
                await ReMoveProductAsync();
                Console.WriteLine(" danh sách sản phẩm đã cập nhật là:.");
                await ReadProductAsync();
                break;
            default:
                Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập 1 hoặc 2.");
                while (true)
                {
                    Console.Write("Nhập lại lựa chọn của bạn (1 hoặc 2) nếu bạn không muốn chọn hãy nhấn 'huy' để thoát: ");
                    choice = Console.ReadLine();
                    if(choice?.Trim() == "1")
                    {
                        Console.WriteLine("Bạn đã chọn nhập sản phẩm.");
                        await InsertProductAsync();
                        await ReadProductAsync();
                        break;
                    } if (choice?.Trim() == "2")
                    {
                        Console.WriteLine("Bạn đã chọn hiển thị danh sách sản phẩm.");
                        await ReadProductAsync();
                        break;
                    }
                    if( choice?.Trim().ToLower() == "huy")
                    {
                        Console.WriteLine("Bạn đã chọn hủy bỏ. Thoát chương trình.");
                        return;
                    }
                    if (choice?.Trim() == "3")
                    {
                        Console.WriteLine("Bạn đã chọn cập nhật danh sách sản phẩm.");
                await UpdateProductAsync();
                Console.WriteLine(" danh sách sản phẩm đã cập nhật là:.");
                await ReadProductAsync();
                break;
                    }
                    Console.WriteLine("Lựa chọn không hợp lệ. Vui lòng nhập 1 , 2 , 3 , huy.");
                }
                return;
        }
        
        
    }

    private async Task CreateDatabaseAsync()
    {
        var created = await _context.Database.EnsureCreatedAsync();
        if (created)
        {
            Console.WriteLine("✅ Cơ sở dữ liệu đã được tạo thành công!");
        }
       

    }
     private async Task ReadProductAsync()
    {
        Console.WriteLine("Danh sách cách sản phẩm có trong cơ sở dữ liệu:");
        Console.WriteLine("--------------------------------------------------");
       var products = await _context.Products.ToListAsync();
        if (products.Count == 0)
        {
            Console.WriteLine("❌ Không có sản phẩm nào trong cơ sở dữ liệu.");
            return;
        }
        products.ForEach(product => product.Display()); 

    }
   private async Task UpdateProductAsync()
{
    Console.Write("Nhập ID sản phẩm cần cập nhật: ");
    if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
    {
        Console.WriteLine("❌ ID sản phẩm không hợp lệ. Vui lòng nhập lại.");
        return;
    }

    // Dùng LINQ để tìm sản phẩm theo ID
    var product = await _context.Products
                                .Where(p => p.ProductId == id)
                                .FirstOrDefaultAsync(); // hoặc SingleOrDefaultAsync()

    if (product == null)
    {
        Console.WriteLine($"❌ Không tìm thấy sản phẩm với ID {id}.");
        return;
    }
    Console.WriteLine($"Sản phẩm hiện tại: ID = {product.ProductId}, Tên = {product.Name}, Nhà cung cấp = {product.Provider}");
    Console.Write("Nhập tên sản phẩm mới: ");
    string? name = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(name))
    {
        Console.WriteLine("❌ Tên sản phẩm không được để trống. Vui lòng nhập lại.");
        return;
    }

    Console.Write("Nhập nhà cung cấp mới: ");
    string? provider = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(provider))
    {
        Console.WriteLine("❌ Nhà cung cấp không được để trống. Vui lòng nhập lại.");
        return;
    }

    // Gán giá trị mới
    product.Name = name;
    product.Provider = provider;

    _context.Products.Update(product);
    await _context.SaveChangesAsync();

    Console.WriteLine($"✅ Đã cập nhật sản phẩm với ID {id} thành công!");
}
    private async Task ReMoveProductAsync()
    {
        Console.Write("Nhập ID sản phẩm cần xóa: ");
        if (!int.TryParse(Console.ReadLine(), out int id) || id <= 0)
        {
            Console.WriteLine("❌ ID sản phẩm không hợp lệ. Vui lòng nhập lại.");
            return;
        }

        // Dùng LINQ để tìm sản phẩm theo ID
        var product = await _context.Products
                                    .Where(p => p.ProductId == id)
                                    .FirstOrDefaultAsync(); // hoặc SingleOrDefaultAsync()

        if (product == null)
        {
            Console.WriteLine($"❌ Không tìm thấy sản phẩm với ID {id}.");
            return;
        }
        Console.WriteLine($"Sản phẩm hiện tại: ID = {product.ProductId}, Tên = {product.Name}, Nhà cung cấp = {product.Provider}");
        Console.Write("Bạn có chắc chắn muốn xóa sản phẩm này? (y/n): ");
        var confirmation = Console.ReadLine()?.Trim().ToLower();
        if (confirmation != "y")
        {
            Console.WriteLine("❌ Hủy bỏ xóa sản phẩm.");
            return;
        }
        else
        {
            Console.WriteLine($"Đang xóa sản phẩm với ID {id}...");
            _context.Products.Remove(product);

        }
        await _context.SaveChangesAsync();
}

    

    private async Task InsertProductAsync()
    {
        var productList = new List<Product>();

        while (true)
        {

            var product = new Product();

            Console.Write("Nhập tên sản phẩm: ");
            product.Name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(product.Name)) continue;

            Console.Write("Nhập nhà cung cấp: ");
            product.Provider = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(product.Provider)) continue;

            productList.Add(product);

            Console.Write("Nhập thành công! Nhấn Enter để tiếp tục hoặc nhập 'xong' để kết thúc: ");
            var input = Console.ReadLine();
            if (input?.Trim().ToLower() == "xong") break;
        }

        await _context.Products.AddRangeAsync(productList);
        await _context.SaveChangesAsync();
        Console.WriteLine($"✅ Đã lưu {productList.Count} sản phẩm thành công!");
    }
}
