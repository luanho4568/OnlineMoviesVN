using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnlineMoviesVN.DAL.Data;
using OnlineMoviesVN.Utility.Constant;
using OnlineMoviesVN.Utility.Cookies;
using OnlineMoviesVN.Utility.JwtAuthentication;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<JwtService>();


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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                context.Response.Redirect("/Admin/Home/Login");
                context.HandleResponse();
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                context.Token = context.Request.GetCookie(StorageConstants.KeyTokenCookie);
                return Task.CompletedTask;
            }

        };
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

app.UseHttpsRedirection();
app.UseStaticFiles();


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
    //else if (response.StatusCode == 500)
    //{
    //    response.Redirect("/notfound");
    //}
    await Task.CompletedTask;
});

app.UseAuthentication();
app.UseAuthorization();



app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Client}/{controller=Home}/{action=Index}/{id?}"
);
app.Run();