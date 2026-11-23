using Lumora.Data;
using Lumora.Infrastructure.Payments;
using Lumora.Infrastructure.Pdf;
using Lumora.Infrastructure.Storage;
using Lumora.Repositories.Implementations;
using Lumora.Repositories.Interfaces;
using Lumora.Services.Implementations;
using Lumora.Services.Interfaces;
using Lumora.Services.Observers;
using Lumora.Services.Strategies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Presentation Tier: MVC Controllers & Razor Views
builder.Services.AddControllersWithViews();

// 2. Data Access Tier: DbContext & SQL Server (Configured dynamically from appsettings.json)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found in appsettings.json.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 3. Data Access Tier: Unit of Work & Repositories
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// 4. Business Logic Tier: Strategy Pattern for Discounts
builder.Services.AddScoped<IDiscountStrategy, PercentageDiscountStrategy>();
builder.Services.AddScoped<IDiscountStrategy, FixedAmountDiscountStrategy>();
builder.Services.AddScoped<IDiscountStrategyFactory, DiscountStrategyFactory>();

// 5. Business Logic Tier: Observer Pattern for Certification
builder.Services.AddScoped<ICourseCompletionObserver, CertificateGenerationObserver>();

// 6. Business Logic Tier: Core Application Services
builder.Services.AddScoped<ICourseCatalogService, CourseCatalogService>();
builder.Services.AddScoped<ICheckoutService, CheckoutService>();
builder.Services.AddScoped<ICertificateVerificationService, CertificateVerificationService>();
builder.Services.AddScoped<IAdminApprovalService, AdminApprovalService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// 7. Infrastructure Tier: Cloud & External Services
builder.Services.AddScoped<ICloudStorageService, AzureBlobStorageService>();
builder.Services.AddScoped<IPaymentGateway, StripePaymentGateway>();
builder.Services.AddScoped<IPdfCertificateGenerator, PdfCertificateGenerator>();

var app = builder.Build();

// Automatically seed initial platform data if empty
using (var scope = app.Services.CreateScope())
{
    var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    await DbSeeder.SeedAsync(uow);
}

// Configure HTTP Request Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Map default route to Home/Index (Public Landing Page)
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
