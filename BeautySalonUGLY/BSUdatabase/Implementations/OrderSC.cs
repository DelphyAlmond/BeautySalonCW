using AutoMapper;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUcontrmodels.Enums;
using BSUdatabase;
using BSUdatabase.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

internal class OrderSC : IOrderSC
{
    private readonly BSUdbContext _dbContext;
    private readonly Mapper _mapper;

    public OrderSC(BSUdbContext context)
    {
        _dbContext = context;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Order, OrderDM>()
            .ConstructUsing(src => new OrderDM(
                src.ID,
                src.CustomerID,
                src.WorkerID,
                src.Cart.Select(link => _mapper.Map<ProdUnitOrderLinkDM>(link)).ToList(),
                src.Discount,
                src.Status,
                src.Summ,
                _mapper.Map<WorkerDM>(src.Worker),
                _mapper.Map<CustomerDM>(src.Customer)
            ));
            cfg.CreateMap<OrderDM, Order>()
            .ForMember(x => x.Status, x => x.MapFrom(src => OrderStatus.Placed))
            .ForMember(dest => dest.Cart, opt => opt.MapFrom(src => src.Cart))
            .AfterMap((src, dest) =>
            {
                if (dest.Cart != null)
                    foreach (var item in dest.Cart)
                        item.OrderID = dest.ID;
            });

            cfg.CreateMap<ProductOrder, ProdUnitOrderLinkDM>()
             .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductNaming))
             .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price));
            cfg.CreateMap<ProdUnitOrderLinkDM, ProductOrder>();

            cfg.CreateMap<Product, ProductDM>();
            cfg.CreateMap<Manufacturer, ManufacturerDM>();

        }, NullLoggerFactory.Instance);
        _mapper = new Mapper(config);
    }

    public List<OrderDM> GetOrdersByDateGap(DateTime? start, DateTime? end, string? workerID, string? customerID, string? productID)
    {
        try
        {
            var query = _dbContext.Orders.Include(o => o.Cart).ThenInclude(o => o.Product)
                .Include(o => o.Worker).Include(o => o.Customer).AsQueryable();

            if (start is not null && end is not null) // DateTime =/= null dif.
                query = query.Where(o => o.Date >= start && o.Date <= end);
            if (workerID is not null) query = query.Where(o => o.WorkerID == workerID);
            if (customerID is not null) query = query.Where(o => o.CustomerID == customerID);
            if (productID is not null)
                query = query.Where(o => o.Cart!.Any(c => c.ProductID == productID));

            return [.. query.Select(o => _mapper.Map<OrderDM>(o))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public List<OrderDM> GetOrdersByStatus(OrderStatus status, DateTime from, DateTime to)
    {
        try
        {
            return [.. _dbContext.Orders.Include(o => o.Cart)
                .ThenInclude(c => c.Product)
                .Include(o => o.Worker)
                .Include(o => o.Customer)
                .Where(o => o.Status == status && o.Date >= from && o.Date <= to)
                .Select(o => _mapper.Map<OrderDM>(o))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public OrderDM? GetOByID(string id)
    {
        try
        {
            return _mapper.Map<OrderDM>(GetOrderByID(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddO(OrderDM order)
    {
        try
        {
            _dbContext.Orders.Add(_mapper.Map<Order>(order));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", order.ID);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdO(OrderDM order)
    {
        try
        {
            var existingOrder = GetOrderByID(order.ID
                ?? throw new Exception($"Order ID is null"))
                ?? throw new Exception($"Order not found: {order.ID}");
            // Получаем смапп.-ую модель типа Order (из Order DataModel)
            _mapper.Map(order, existingOrder);
            // 1. Обновляем коллекцию Cart
            _dbContext.Entry(existingOrder).Collection(o => o.Cart!).Load();
            // 2. Удаляем старые элементы
            _dbContext.ProductUnits.RemoveRange(existingOrder.Cart);
            // 3. Добавляем новые
            var newCart = order.Cart.Select(link => _mapper.Map<ProductOrder>(link)).ToList();
            foreach (var item in newCart)
                item.OrderID = existingOrder.ID;
            existingOrder.Cart = newCart;

            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void DelO(string id)
    {
        try
        {
            var entity = GetOrderByID(id) ?? throw new Exception($"< * > ObjOrder not found: {id}");
            if (entity.Status == OrderStatus.Canceled)
            {
                throw new Exception("< ! > Element(Order) was canceled earlier (not actual anymore)");
            }
            entity.Status = OrderStatus.Placed;
            _dbContext.Orders.Remove(entity);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Order? GetOrderByID(string id) =>
        _dbContext.Orders.Include(o => o.Cart).ThenInclude(c => c.Product)
        .Include(o => o.Worker)
        .Include(o => o.Customer)
        .FirstOrDefault(o => o.ID == id); // < здесь без фильтрации на актуальность
}
