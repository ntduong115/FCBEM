
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Models.Authorize;
using Core.Commons.Policy;
using static Core.Commons.FCConstants;
using Core.Models.Utility;
using Core.Services;
using Core.Interfaces;
using MasterCore.Commons.Extentions;
using MasterCore.DbContexts;
using FCCore.Middlewares;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", reloadOnChange: true, optional: true)
                .AddJsonFile($"appsettings.json", reloadOnChange: true, optional: true);

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation().AddViewLocalization().AddDataAnnotationsLocalization();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddOptions();

var name = builder.Configuration["ProjectName"];

string? connectstring = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContextPool<DatabaseContext>(options =>
{
    options.UseSqlServer(connectstring, b => b.MigrationsAssembly(name)).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

});

string? connectstringLog = builder.Configuration.GetConnectionString("Log");
builder.Services.AddDbContextPool<LogContext>(options =>
{
    options.UseSqlServer(connectstringLog).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddFilter((category, level) => category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information)));

});

builder.Logging.AddDbLogger(options =>
{
    IConfigurationSection configurationSection = builder.Configuration.GetSection("Logging:Database:Options");
    configurationSection.GetSection("ConnectionString").Value = connectstring;
    configurationSection.Bind(options);
});

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Over18", policy => policy.Requirements.Add(new Over18Requirement()))
    .AddPolicy(PolicyConstant.LoginOnly, policy => policy.Requirements.Add(new LoginOnlyRequirement()))
    .AddPolicy("Role", policy => policy.Requirements.Add(new RoleRequirement()));

// Cấu hình Data Protection
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo($@"C:\keys\{name}")) // Thay đổi đường dẫn tới thư mục nơi bạn muốn lưu key ring
    .SetApplicationName(name) // Đảm bảo tất cả các ứng dụng sử dụng cùng tên ứng dụng
    .SetDefaultKeyLifetime(TimeSpan.FromDays(90)); // Thời gian hiệu lực của một key


builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 1;

    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //options.Lockout.MaxFailedAccessAttempts = 5;
    //options.Lockout.AllowedForNewUsers = true;

    options.User.AllowedUserNameCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    options.User.RequireUniqueEmail = true;

    //options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;

}).AddEntityFrameworkStores<DatabaseContext>().AddDefaultTokenProviders();

builder.Services.AddTransient<IEmailSender, SendMailService>();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = $"{name}.Identity";
    options.Cookie.Path = $"/{name}";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    //options.ExpireTimeSpan = TimeSpan.FromDays(1); // Thời gian hết hạn của cookie
    //options.SlidingExpiration = true; // Cho phép thời gian hết hạn được kéo dài khi có hoạt động từ người dùng
    //options.LoginPath = $"/{name}/Login";
    //options.LogoutPath = $"/{name}/Logout";
    //options.AccessDeniedPath = $"/{name}/AccessDenied";
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1); // Thời gian hết hạn của session
    options.Cookie.HttpOnly = true; // Cookie chỉ được truy cập qua HTTP (không thể truy cập từ JavaScript)
    options.Cookie.IsEssential = true; // Cookie này cần thiết cho ứng dụng
    options.Cookie.Path = $"/{name}";
});

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Name = $"{name}.Antiforgery";
    options.HeaderName = $"{name}.Header";
    options.Cookie.Path = $"/{name}";
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
}
// thuantv8 add
var option = new RewriteOptions()
        .AddRedirect("^(.*)/tour", "$1/comingsoon", 301)
        .AddRedirect("^(.*)/visa", "$1/comingsoon", 301)
        .AddRedirect("^(.*)/program$", "$1/comingsoon", 301); // 301 for Permanent redirect  


app.UseRewriter(option);


app.UsePathBase($"/{name}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AntiforgeryCookieMiddleware>();
app.UseMiddleware<ExceptionLoggingMiddleware>();

app.MapRazorPages();

app.Run();

