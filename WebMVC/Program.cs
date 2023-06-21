using ImageUploaderWebMVC.Data;
using ImageUploaderWebMVC.Repositories;
using ImageUploaderWebMVC.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using System;
using static System.Formats.Asn1.AsnWriter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the blob container.
IConfiguration config = builder.Configuration;
builder.Services.AddTransient<IStorageService, StorageService>();
builder.Services.AddAzureClients(clientbuilder =>
{
    clientbuilder.AddBlobServiceClient(builder.Configuration.GetSection("AzureStorage:ConnectionString").Value);
});

//Connection to database 
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("SqlConnection")));

//Add UnitOfWork
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

//Add ImageService
builder.Services.AddScoped<IImageService, ImageService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Images}/{action=Index}/{id?}");

// Migrate latest database changes during startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<AppDbContext>();

    // Here is the migration executed
    dbContext.Database.Migrate();
}

app.Run();

