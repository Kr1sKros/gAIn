using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using gain.Data;
using gain.Models;
using Microsoft.AspNetCore.Authentication;
using OpenAITest.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using gain.Services;


// Cookie.clear()
// When recompiled without signout, browser still stores authentication cookie
// Allows acces to [Authorize] pages and leads to runtime error when trying to ger user info

var builder = WebApplication.CreateBuilder(args);
var googleServices = builder.Services;
var configuration = builder.Configuration;

// Uses google package ver 8.0.10
googleServices.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
});



// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<OpenAIService>();

// SendGrid ver 9.29.3
// Features missing from latest .NET 8 version 8.0.5
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);


var app = builder.Build();

using (var services = app.Services.CreateScope()){
            
    var db = services.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    
    var um = services.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var rm = services.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    ApplicationDbInitializer.Initialize(db, um, rm);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Måtte legge til denne linja iølge GPT
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
