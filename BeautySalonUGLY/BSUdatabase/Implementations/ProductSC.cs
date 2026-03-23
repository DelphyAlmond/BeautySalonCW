using AutoMapper;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUdatabase;
using BSUdatabase.DBModels;
using BSUmodels.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;

internal class ProductSC : IProductSC
{
    private readonly BSUdbContext _dbContext;
    private readonly Mapper _mapper;

    public ProductSC(BSUdbContext context)
    {
        _dbContext = context;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Product, ProductDM>();
            // Изменения должны приходить от тех объектов сущности,
            // которые на данный момент актуальны и НЕ удалены, поэтому + фильтрация:
            cfg.CreateMap<ProductDM, Product>().ForMember(x => x.IsDeleted,
                                                          x => x.MapFrom(src => false));
            // [ + ]
            cfg.CreateMap<Manufacturer, ManufacturerDM>();
            // > Теперь можем вместе с самим изделием -> цеплять производителя

        }, NullLoggerFactory.Instance);
        _mapper = new Mapper(config);
    }

    public List<ProductDM> GetProducts(bool onlyActive = true, string? manufacturerID = null)
    {
        try
        {
            // По принципу отложенного запроса
            // (постепенно формируется выборка за счёт фильтров):

            var query = _dbContext.Products.Include(m => m.Manufacturer).AsQueryable();

            if (onlyActive) query = query.Where(p => !p.IsDeleted);
            if (!manufacturerID.IsEmpty())
                query = query.Where(p => p.ManufacturerID == manufacturerID);
            // query.Select(p => _mapper.Map<ProductDM>(p)).ToList() или
            return [.. query.Select(p => _mapper.Map<ProductDM>(p))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ProductDM? GetItemByID(string id)
    {
        try
        {
            return _mapper.Map<ProductDM>(GetProductByID(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ProductDM? GetItemByName(string name)
    {
        try
        {
            return _mapper.Map<ProductDM>(
                _dbContext.Products.Include(m => m.Manufacturer).FirstOrDefault(p => p.ProductNaming == name && !p.IsDeleted));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddItem(ProductDM item)
    {
        try
        {
            _dbContext.Products.Add(_mapper.Map<Product>(item));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", item.ID);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Products_ProductNaming" })
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("ProductName", item.ProductNaming);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdItem(ProductDM item)
    {
        try
        {
            var entity = GetProductByID(item.ID
                ?? throw new Exception($"< * > ObjProduct not found: {item.ProductNaming}"));
            _mapper.Map(item, entity);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Products_ProductNaming" })
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("ProductName", item.ProductNaming);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void DelItem(string id)
    {
        try
        {
            var entity = GetProductByID(id) ?? throw new Exception($"< * > ObjProduct not found: {id}");
            // Мягкое удаление [ ! ]
            entity.IsDeleted = true;
            // (в случае надобности - если уже удалён : выкинуть ошибку)
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Product? GetProductByID(string id) =>
        _dbContext.Products.Include(m => m.Manufacturer).FirstOrDefault(p => p.ID == id && !p.IsDeleted);
}