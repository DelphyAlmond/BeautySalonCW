using AutoMapper;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUdatabase;
using BSUdatabase.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;

internal class ServiceSC : IServiceSC
{
    private readonly BSUdbContext _dbContext;
    private readonly Mapper _mapper;

    public ServiceSC(BSUdbContext context)
    {
        _dbContext = context;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Service, ServiceDM>();
            // Ожидание изменений от актуальных, НЕ удаленых, записей - фильтрация:
            cfg.CreateMap<ServiceDM, Service>().ForMember(x => x.IsDeleted,
                                                          x => x.MapFrom(src => false));
        }, NullLoggerFactory.Instance);
        _mapper = new Mapper(config);
    }

    public List<ServiceDM> GetServices(bool onlyActive = true)
    {
        try
        {
            IQueryable<Service> query = _dbContext.Services;
            // или var query = _dbContext.Services.AsQueryable();
            if (onlyActive) query = query.Where(s => !s.IsDeleted);
            return query.Select(s => _mapper.Map<ServiceDM>(s)).ToList();
            // или [.. query.Select(s => _mapper.Map<ServiceDM>(s))]
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ServiceDM? GetSByID(string id)
    {
        try
        {
            return _mapper.Map<ServiceDM>(GetServiceByID(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ServiceDM? GetSByName(string name)
    {
        try
        {
            return _mapper.Map<ServiceDM>(                                     // * s.IsDeleted == false
                _dbContext.Services.FirstOrDefault(s => s.ServiceNaming == name && !s.IsDeleted));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddElement(ServiceDM service)
    {
        try
        {
            _dbContext.Services.Add(_mapper.Map<Service>(service));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", service.ID);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Services_ServiceNaming" })
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("ServiceName", service.ServiceNaming);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdElement(ServiceDM service)
    {
        try
        {
            var entity = GetServiceByID(service.ID
                ?? throw new Exception($"< * > ObjService not found: {service.ServiceNaming}"));
            _mapper.Map(service, entity);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Services_ServiceNaming" })
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("ServiceName", service.ServiceNaming);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void DelElement(string id)
    {
        try
        {
            var entity = GetServiceByID(id) ?? throw new Exception($"< * > ObjService not found: {id}");
            entity.IsDeleted = true; // Тоже мягкое удаление [ ! ]
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Service? GetServiceByID(string id) =>
        _dbContext.Services.FirstOrDefault(s => s.ID == id && !s.IsDeleted);
}
