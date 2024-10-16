using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using SignupApp.Models;
using SignupApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Optional: specify the HTTPS port if necessary
builder.WebHost.UseUrls("http://localhost:5182;https://localhost:5183");


// Load MongoDB settings from appsettings.json
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbSettings"));

// Register UserService
builder.Services.AddSingleton<UserService>();

// Add MVC services
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Custom error page
    app.UseHsts(); // Use HSTS in production
}

// Use HTTPS redirection and static files
app.UseHttpsRedirection();
app.UseStaticFiles();

// Routing configuration
app.UseRouting();

// Authorization middleware
app.UseAuthorization();

// Define endpoints
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
