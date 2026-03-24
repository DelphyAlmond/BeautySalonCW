using BSUcontractmodels.Infrastructure;
using BSUdatabase.DBModels;
using Microsoft.EntityFrameworkCore;

namespace BSUdatabase;

internal class BSUdbContext : DbContext
{
    private readonly IConfigurationDatabase? _configDB;

    public BSUdbContext(IConfigurationDatabase configurationDatabase)
    {
        _configDB = configurationDatabase;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    // Подкл. к БД-х:
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {      // ^ перегружаем метод (вызываемый при конфигурации проекта)

        optionsBuilder.UseNpgsql(_configDB.ConnectionStr, o => o.SetPostgresVersion(12, 2));
        base.OnConfiguring(optionsBuilder);
    }

    // Декларация уникальности для полей объектов-классов(моделей):
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>().HasIndex(x => x.Phonenumber).IsUnique();
        modelBuilder.Entity<Manufacturer>().HasIndex(x => x.CurrentName).IsUnique();
        // Составной критерий уникальности (только у действующих записей,
        // а в удалённых устаревшая информация - не требующая проверки):
        modelBuilder.Entity<Product>()
            .HasIndex(x => new { x.ProductNaming, x.IsDeleted }).IsUnique()
            .HasFilter($"\"{nameof(Product.IsDeleted)}\" = FALSE"); // if isActual - w'd be TRUE [ * ]

        // Запр. удаление производителя, у которого \ к которому привязаны продукты:
        modelBuilder.Entity<Product>()
            .HasOne(x => x.Manufacturer).WithMany(p => p.Products)
            .OnDelete(DeleteBehavior.Restrict);

        // Для сущностей Заказа-Покупки и Посещения-Предоставл.Услуги проверка за счёт составного ключа
        modelBuilder.Entity<ProductOrder>().HasKey(x => new { x.OrderID, x.ProductID });
        modelBuilder.Entity<ServiceVisit>().HasKey(x => new { x.VisitID, x.ServiceID });
    }

    public DbSet<Customer> Cusromers { get; set; }
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Manufacturer> Manufacturers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ProductOrder> ProductUnits { get; set; }
    public DbSet<ServiceVisit> ServiceUnits { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Visit> Visits { get; set; }
}
