using BSUbusinesslogic.Implementations;
using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.Infrastructure;
using BSUcontractmodels.StoragesContracts;
using BSUdatabase;
using BSUdatabase.Implementations;
using BSUWebAppi.InfrastructureDB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// 0. Установка логгера
using var loggerFactory = new LoggerFactory();
loggerFactory.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger());
builder.Services.AddSingleton(loggerFactory.CreateLogger("Any"));
// edit the appsettings.json next further [ * ]

// 1.* настройка аутентификации
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
    options.TokenValidationParameters = new TokenValidationParameters
    { 
    // указывает, будет ли валидироваться издатель при валидации токена
    ValidateIssuer = true, 
    // строка, представляющая издателя 
    ValidIssuer = AuthOptions.ISSUER, 
    // будет ли валидироваться потребитель токена 
    ValidateAudience = true, 
    // установка потребителя токена 
    ValidAudience = AuthOptions.AUDIENCE, 
    // будет ли валидироваться время существования 
    ValidateLifetime = true, 
    // установка ключа безопасности 
    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(), 
    // валидация ключа безопасности 
    ValidateIssuerSigningKey = true,
    };
});

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// 2.* настройка БД
// Configure the HTTP request pipeline. (для тестов)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
// [ + ]* for BSUdatabase, BSUcontractmodels & BSUbusinesslogic .csproj-s
// < ItemGroup >
//        < InternalsVisibleTo Include = "BSUWebAppi" />
// </ ItemGroup >

// (для проды и миграций)
if (app.Environment.IsProduction())
{
    var dbContext = app.Services.GetRequiredService<BSUdbContext>();
    if (dbContext.Database.CanConnect())
    {
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
    }
}

// 3. этап создания ioc-ра и связки контрактов-реализаций
builder.Services.AddSingleton<IConfigurationDatabase, ConfigurationDB>();

builder.Services.AddTransient<ICustomerBLC, CustomerBLC>();
builder.Services.AddTransient<IManufacturerBLC, ManufacturerBLC>();
builder.Services.AddTransient<IProductBLC, ProductBLC>();
builder.Services.AddTransient<IOrderBLC, OrderBLC>();
builder.Services.AddTransient<IServiceBLC, ServiceBLC>();
builder.Services.AddTransient<IVisitBLC, VisitBLC>();
builder.Services.AddTransient<IWorkerBLC, WorkerBLC>();

builder.Services.AddTransient<BSUdbContext>();
builder.Services.AddTransient<ICustomerSC, CustomerSC>();
builder.Services.AddTransient<IManufacturerSC, ManufacturerSC>();
builder.Services.AddTransient<IServiceSC, ServiceSC>();
builder.Services.AddTransient<IProductSC, ProductSC>();
builder.Services.AddTransient<IOrderSC, OrderSC>();
builder.Services.AddTransient<IVisitSC, VisitSC>();
builder.Services.AddTransient<IWorkerSC, WorkerSC>();


app.UseHttpsRedirection();

// 1.* настройка аутентификации
app.UseAuthentication(); // [ + ]
app.UseAuthorization();

app.Map("login/{username}", (string username) =>
{
    // здесь будет прописан "регламент", запрос, с помощью которого осущесвиться:
    // проверка того, кто приходит, есть ли у него доступ;
    // считывание его данных и дальнейщее их хранение, таких как
    // логин-пароль и логика для их проверки на связь в отд. классе;
});

app.MapControllers();

app.Run();
