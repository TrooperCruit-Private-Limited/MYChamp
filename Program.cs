using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MYChamp.Controller;
using MYChamp.DbContexts;
using MYChamp.Models;
using System;
using System.Net.Http;
using Microsoft.OpenApi.Models;
using MYChamp.Pages; // Add this using directive

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<EmailSenderService>();
builder.Services.AddDbContext<MYChampDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("TrooperCruitPostgreSQL")));

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequiredUniqueChars = 0;
    options.Password.RequireUppercase = true;
})
.AddEntityFrameworkStores<MYChampDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
});

builder.Services.AddHttpContextAccessor(); // Register HttpContextAccessor here

builder.Services.AddScoped<LoginController>();
builder.Services.AddScoped<SessionHandlerController>();
//builder.Services.AddHostedService<ForcefulLogoutBackgroundService>();

builder.Services.AddHttpClient();
 
// Register the Swagger generator
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

app.UseSession();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
     
app.Run();
