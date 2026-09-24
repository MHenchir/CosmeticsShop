using CosmeticsShop.Api.Middleware;
using CosmeticsShop.Application.Abstractions;
using CosmeticsShop.Application.Carts;
using CosmeticsShop.Application.Categories;
using CosmeticsShop.Application.Customers;
using CosmeticsShop.Application.Orders;
using CosmeticsShop.Application.Products;
using CosmeticsShop.Infrastructure.Persistence;
using CosmeticsShop.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using CosmeticsShop.Application.Chatbot;
using CosmeticsShop.Infrastructure.Chatbot;

var builder = WebApplication.CreateBuilder(args);

// --- Base de données ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Repositories (Infrastructure) : "quand on demande l'interface, donne l'implémentation concrète" ---
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
// --- Chatbot ---
builder.Services.AddHttpClient<IChatCompletionProvider, OpenAiChatCompletionProvider>();


// --- Services (Application) ---
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddTransient<GlobalExceptionMiddleware>();
builder.Services.AddScoped<ChatService>();
// --- ASP.NET Core ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- CORS : autoriser Angular (localhost:4200) à appeler cette Api ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthorization();
app.MapControllers();

app.Run();