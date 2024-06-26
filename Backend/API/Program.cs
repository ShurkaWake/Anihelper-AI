using System.Text.Json.Serialization;
using API.Extenstions;
using API.Mapping;
using AutoMapper;
using BusinessLogic.Abstractions;
using BusinessLogic.Mapping;
using DataAccess;
using DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

// Add services to the container.

builder.Services.AddRazorPages();

services
    .AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

string connectionString = builder.Configuration["DbConnectionString"];
services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(connectionString, options =>
    {
        options.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName);
    });
});
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

services
    .AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<ApplicationContext>()
    .AddDefaultTokenProviders();

services.AddServicesOptions(configuration);

services.AddBusinessLogicServices();
services.AddBearerAuthentication();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddSwagger();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new ApiProfile());
    mc.AddProfile(new BusinessProfile());
});

services.AddSingleton(mapperConfig.CreateMapper());

services.AddCors(c =>
{
    c.AddPolicy("DefaultPolicy", p =>
    {
        p.AllowAnyMethod();
        p.AllowAnyOrigin();
        p.AllowAnyHeader();
    });
});

var app = builder.Build();

await app.Services.CreateScope().ServiceProvider.GetRequiredService<ISeeder>().SeedAsync();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseCors("DefaultPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapDefaultControllerRoute();

app.Run();