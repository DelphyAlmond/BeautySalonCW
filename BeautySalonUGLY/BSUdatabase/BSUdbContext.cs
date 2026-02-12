using BSUdatabase.DBModels;
using Microsoft.EntityFrameworkCore;

namespace BSUdatabase;

internal class BSUdbContext
{
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
