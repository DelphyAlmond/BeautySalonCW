using BSUbusinesslogic.Implementations;
using BSUbusinesslogic.OfficePackage;
using BSUcontractmodels.AdapterContracts;
using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.Infrastructure;
using BSUcontractmodels.OfficePackage;
using BSUcontractmodels.StoragesContracts;
using BSUdatabase;
using BSUdatabase.Implementations;
using BSUWebAppi;
using BSUWebAppi.Adapters;
using BSUWebAppi.InfrastructureDB;
using BSUWebAppi.MapProfiles;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;

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

// 1.* AutoMapper : VM <-> DM
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<ReportProfile>();
});

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


builder.Services.AddTransient<ICustomerAdapter, CustomerAdapter>();
builder.Services.AddTransient<IManufacturerAdapter, ManufacturerAdapter>();
builder.Services.AddTransient<IServiceAdapter, ServiceAdapter>();
builder.Services.AddTransient<IProductAdapter, ProductAdapter>();
builder.Services.AddTransient<IOrderAdapter, OrderAdapter>();
builder.Services.AddTransient<IVisitAdapter, VisitAdapter>();
builder.Services.AddTransient<IWorkerAdapter, WorkerAdapter>();

builder.Services.AddTransient<IReportAdapter, ReportAdapter>();
builder.Services.AddTransient<IReportBLC, ReportBLC>();
builder.Services.AddTransient<IReportDocumentBLC, ReportDocumentBLC>();
builder.Services.AddTransient<IWordBuilder, WordReportBuilder>();
builder.Services.AddTransient<IEmailSenderBLC, EmailSenderBLC>();

builder.Services.AddTransient<IAuthenticationBLC, AuthenticationBLC>();
builder.Services.AddTransient<IUserSC, UserSC>();

// 4. Регистрация сервисов аутентификации
builder.Services.AddTransient<IAuthenticationBLC, AuthenticationBLC>();
builder.Services.AddTransient<IUserSC, UserSC>();

app.UseHttpsRedirection();

// 1.* настройка аутентификации
app.UseAuthentication(); // [ + ]
app.UseAuthorization();

app.Map("login/{username}", (string username) =>
{
    // > POST /api/authentication/login :
    // Данный endpoint оставлен для обратной совместимости
    // см. AuthenticationController.cs для полноценной системы аутентификации

    return Results.Redirect("/swagger");
});

app.MapControllers();

app.Run();
