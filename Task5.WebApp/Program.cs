using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Task5.Database.DbContext;
using Task5.Database.Entities;
using Task5.Database.Repositories;
using Task5.WebApi.Services.Mappers;
using Task5.Services.Users;
using Task5.Services.Authentication;
using Task5.Services.Verification;
using Task5.Services.Emails;
using Microsoft.AspNetCore.Mvc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<UserDbContext>(_options =>
{
    _ = _options.UseSqlite(builder.Configuration.GetConnectionString("UserDbConnection"));
});

builder.Services.AddIdentity<UserEntity, IdentityRole>()
                .AddEntityFrameworkStores<UserDbContext>()
                .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/Login";
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;           
    options.Password.RequireLowercase = false;        
    options.Password.RequireUppercase = false;        
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequiredLength = 1;             
    options.Password.RequiredUniqueChars = 1;        
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
});

builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<IVerificationService, EmailVerificationService>();

// Email API config
var gmailAppPassword = builder.Configuration["Gmail:appPassword"];
var gmailMailbox = builder.Configuration["Gmail:Email"];
var gmailName = builder.Configuration["Gmail:Name"];

builder.Services.AddSingleton<IEmailSender>(opt => new GmailSender(gmailMailbox, gmailName, gmailAppPassword));

builder.Services.AddAutoMapper(cfg => { }, typeof(UserMapper));

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
    db.Database.Migrate();
}

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