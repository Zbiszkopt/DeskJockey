using Azure.Storage.Blobs;
using Azure.Storage;
using DeskJockey.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Cookies";
    options.DefaultSignInScheme = "Cookies";
    options.DefaultChallengeScheme = "Cookies";
})
.AddCookie("Cookies", options =>
{
    options.Cookie.Name = "YourAppCookieName";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
});

var configuration = builder.Configuration;
string accountName = configuration["AzureBlobStorage:AccountName"]
                     ?? throw new InvalidOperationException("Azure Blob Storage account name is not configured.");
string accountKey = configuration["AzureBlobStorage:AccountKey"]
                     ?? throw new InvalidOperationException("Azure Blob Storage account key is not configured.");

var blobServiceClient = new BlobServiceClient(
    new Uri($"https://{accountName}.blob.core.windows.net"),
    new StorageSharedKeyCredential(accountName, accountKey)
);

builder.Services.AddSingleton(blobServiceClient);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "images",
    pattern: "Image/ListImages",
    defaults: new { controller = "Image", action = "ListImages" });

app.MapControllerRoute(
    name: "login",
    pattern: "Account/Login",
    defaults: new { controller = "Login", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();