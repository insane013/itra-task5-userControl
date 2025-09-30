using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task5.Database.DbContext;
using Task5.Database.Entities;
using Task5.Database.Repositories;
using Task5.WebApp.Controllers;
using AutoMapper;
using Task5.WebApi.Services.Mappers;
using Microsoft.AspNetCore.Mvc;
using Task5.Services.Abstraction;
using Task5.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<UserDbContext>(_options =>
{
    _ = _options.UseSqlServer(builder.Configuration.GetConnectionString("UserDbConnection"));
});

builder.Services.AddIdentity<UserEntity, IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/AccessDenied";
});     

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddAutoMapper(cfg => { }, typeof(UserMapper));

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();