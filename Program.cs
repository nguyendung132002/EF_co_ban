using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using clasProduct;
using ProducDbContext;

var builder = Host.CreateApplicationBuilder(args);

// ✅ Đăng ký DbContext sử dụng DI
  builder.Services.AddDbContext<ProducDbContext.ProducDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

builder.Services.AddTransient<App>(); // Đăng ký App là dịch vụ thực thi logic
var app = builder.Build();
using var scope = app.Services.CreateScope();
var program = scope.ServiceProvider.GetRequiredService<App>();
await program.RunAsync();
