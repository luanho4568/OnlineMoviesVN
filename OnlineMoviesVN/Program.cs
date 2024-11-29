using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.DAL.Repository;
using OnlineMoviesVN.DAL.Repository.IRepository;
using OnlineMoviesVN.Utility.Constant;
using OnlineMoviesVN.Utility.Cookies;
using OnlineMoviesVN.Utility.JwtAuthentication;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(20);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("Token xác thực thất bại: " + context.Exception.Message);

                return Task.CompletedTask;
            },
            OnTokenValidated = async context =>
            {
                var jwtService = context.HttpContext.RequestServices.GetService<JwtService>();


                var expireTokenJwt = jwtService.ValidateTokenExpiration(context.Principal, context.HttpContext);
                if (!expireTokenJwt)
                {
                    Console.WriteLine("Token hết hạn");
                }
                else
                {
                    await jwtService.RenewSession(context.HttpContext);
                    Console.WriteLine($"Xác thực user {context.Principal.Identity.Name} thành công: ");
                }
            },
            OnChallenge = context =>
            {
                Console.WriteLine("Chưa có user đăng nhập");
                context.Response.Redirect("/Account/Login");
                context.HandleResponse();
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                context.Token = context.Request.GetCookie(StorageConstants.KeyTokenCookie);
                return Task.CompletedTask;
            }

        };
    })

    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    })
   .AddGoogle(googleOptions =>
   {
       googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
       googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
       googleOptions.CallbackPath = new PathString("/signin-google");
       googleOptions.Events.OnTicketReceived = async context =>
        {
            // Lấy thông tin email từ Google
            var email = context.Principal.FindFirstValue(ClaimTypes.Email);
            // Xác thực trong cơ sở dữ liệu
            var userRepo = context.HttpContext.RequestServices.GetService<IUnitOfWork>();
            var user = await userRepo.User.GetFirstOrDefaultAsync(u => u.Email == email && u.AccountType == AccountTypeConstants.Google);

            if (user == null)
            {
                context.Response.Redirect("/Account/Login");
                context.HandleResponse();
                return;
            }

            var identity = context.Principal.Identity as ClaimsIdentity;
            identity.AddClaim(new Claim(ClaimTypes.Role, user.Role));

            // Lưu vào session hoặc cookie
            var jwtService = context.HttpContext.RequestServices.GetService<JwtService>();
            var token = jwtService.GenerateToken(user);
            context.Response.SetCookie(StorageConstants.KeyTokenCookie, token, 7);
        };
   })
   .AddFacebook(facebookOptions =>
   {
       facebookOptions.AppId = configuration["Authentication:Facebook:AppId"];
       facebookOptions.AppSecret = configuration["Authentication:Facebook:AppSecret"];
       facebookOptions.CallbackPath = new PathString("/signin-facebook");
       facebookOptions.Fields.Add("email");
       facebookOptions.Fields.Add("name");
       facebookOptions.Scope.Add("email");

   });


builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedProto
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;

    if (!string.IsNullOrEmpty(path) && !path.EndsWith("/") && !Path.HasExtension(path) && !path.EndsWith("/signin-google") && !path.EndsWith("/signin-facebook"))
    {
        context.Response.Redirect($"{path}/", true); // Chuyển hướng 301
        return;
    }
    await next();
});

app.UseRouting();
app.UseSession();
app.UseStatusCodePages(async context =>
{
    var response = context.HttpContext.Response;
    if (response.StatusCode == 403 || response.StatusCode == 404)
    {
        var requestPath = context.HttpContext.Request.Path.Value?.ToLower();
        var routeData = context.HttpContext.GetRouteData();

        if (routeData?.Values?.Count > 0 && routeData.Values.ContainsKey("controller") && routeData.Values.ContainsKey("action"))
        {
            // Nếu không có controller hoặc action trong route, có thể là tệp tĩnh, bỏ qua chuyển hướng
            // Thực hiện chuyển hướng về trang chủ
            response.Redirect("/");
        }
    }
    else if (response.StatusCode == 500)
    {
        response.Redirect("/notfound");
    }
    await Task.CompletedTask;
});

app.UseAuthentication();
app.UseAuthorization();



app.MapRazorPages();
app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "NotFound",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Client}/{controller=Home}/{action=Index}/{id?}");

app.Run();