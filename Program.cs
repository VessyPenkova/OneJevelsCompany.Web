using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using OneJevelsCompany.Web.Data;
using OneJevelsCompany.Web.Services.Cart;
using OneJevelsCompany.Web.Services.Inventory;
using OneJevelsCompany.Web.Services.Orders;
using OneJevelsCompany.Web.Services.Payment;
using OneJevelsCompany.Web.Services.Product;
using OneJevelsCompany.Web.Services.Dashboard;
using OneJevelsCompany.Web.Services.Common;


var builder = WebApplication.CreateBuilder(args);

// MVC + Razor Pages (Identity UI uses Razor Pages)
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// EF Core — SQL Server (supports ActiveConnection switch)
var activeConnKey = builder.Configuration["ActiveConnection"] ?? "DefaultConnection";
var connectionString = builder.Configuration.GetConnectionString(activeConnKey)
    ?? throw new InvalidOperationException($"Connection string '{activeConnKey}' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


// Identity (Default UI + Roles)
builder.Services
    .AddDefaultIdentity<IdentityUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

// Session (cart)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".OneJevelsCompany.Session";
    options.IdleTimeout = TimeSpan.FromHours(12);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Domain services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, StripePaymentService>();

// Inventory (only if you added it)
builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddScoped<IDashboardService, DashboardService>();

builder.Services.AddScoped<IImageStorage, DiskImageStorage>();
builder.Services.AddScoped<ICategoryLookup, CategoryLookup>();

var app = builder.Build();

// Apply migrations, data seed, identity seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
    await SeedData.ApplyAsync(db);
    await IdentitySeed.ApplyAsync(app.Services);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// Identity UI endpoints
app.MapRazorPages();

app.Run();
