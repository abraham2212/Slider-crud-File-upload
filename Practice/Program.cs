using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Services.Interfaces;
using Practice.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddDbContext<AppDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<ILayoutService, LayoutService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IExpertService, ExpertService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IFooterService, FooterService>();

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
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
