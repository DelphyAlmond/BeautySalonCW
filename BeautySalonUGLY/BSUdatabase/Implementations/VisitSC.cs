using AutoMapper;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUdatabase;
using BSUdatabase.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

internal class VisitSC : IVisitSC
{
    private readonly BSUdbContext _dbContext;
    private readonly Mapper _mapper;

    public VisitSC(BSUdbContext context)
    {
        _dbContext = context;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<ServiceVisit, ServUnitVisitLinkDM>();
            cfg.CreateMap<ServUnitVisitLinkDM, ServiceVisit>();

            cfg.CreateMap<Visit, VisitDM>()
             .ConstructUsing(src => new VisitDM(
                    src.ID,
                    src.CustomerID,
                    src.OrderID,
                    src.WorkerID,
                    src.MasterID,
                    src.DatePlanned,
                    src.Services.Select(link => _mapper.Map<ServUnitVisitLinkDM>(link)).ToList(),
                    src.Discount,
                    src.IsCanceled,
                    src.Summ,
                    _mapper.Map<WorkerDM>(src.Worker),
                    _mapper.Map<WorkerDM>(src.Master),
                    _mapper.Map<CustomerDM>(src.Customer)
             ));
            cfg.CreateMap<VisitDM, Visit>()
            .ForMember(x => x.IsCanceled, x => x.MapFrom(src => false))
            .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Services))
            .AfterMap((src, dest) =>
            {
                if (dest.Services != null)
                    foreach (var item in dest.Services)
                        item.VisitID = dest.ID;
            });

            cfg.CreateMap<Service, ServiceDM>();

        }, NullLoggerFactory.Instance);
        _mapper = new Mapper(config);
    }

    public List<VisitDM> GetVisitsByDateGap(DateTime? start, DateTime? end, string? masterID, string? customerID, string? serviceID)
    {
        try
        {
            IQueryable<Visit> query = _dbContext.Visits.Include(v => v.Services).ThenInclude(v => v.Service)
                .Include(v => v.Worker).Include(v => v.Master).Include(v => v.Customer);

            // ^ ~.AsQueryable();

            // * Фильтры:
            if (start is not null && end is not null) // DateTime =/= null dif.
                query = query.Where(v => v.DatePlanned >= start && v.DatePlanned <= end);
            
            if (masterID is not null) query = query.Where(v => v.WorkerID == masterID);

            if (customerID is not null) query = query.Where(v => v.CustomerID == customerID);

            if (serviceID is not null)
                query = query.Where(v => v.Services!.Any(s => s.ServiceID == serviceID));

            return [.. query.Select(v => _mapper.Map<VisitDM>(v))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public VisitDM? GetVByID(string id)
    {
        try
        {
            return _mapper.Map<VisitDM>(GetVisitByID(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddV(VisitDM visit)
    {
        try
        {
            _dbContext.Visits.Add(_mapper.Map<Visit>(visit));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", visit.ID);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdV(VisitDM visit)
    {
        try
        {
            var existingVisit = GetVisitByID(visit.ID
                ?? throw new Exception($"Visit ID is null"))
                ?? throw new Exception($"Visit not found: {visit.ID}");

            _mapper.Map(visit, existingVisit);

            // Обновляем коллекцию Services
            _dbContext.Entry(existingVisit).Collection(v => v.Services!).Load();
            _dbContext.ServiceUnits.RemoveRange(existingVisit.Services);
            var newServices = visit.Services.Select(s => _mapper.Map<ServiceVisit>(s)).ToList();
            foreach (var item in newServices)
                item.VisitID = existingVisit.ID;
            existingVisit.Services = newServices;

            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void DelV(string id)
    {
        try
        {
            var entity = GetVisitByID(id) ?? throw new Exception($"< * > ObjVisit not found: {id}");
            if (entity.IsCanceled)
            {
                throw new Exception("< ! > Element(Visit) is deleted (not actual anymore)");
            }
            entity.IsCanceled = true;
            _dbContext.Visits.Remove(entity);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Visit? GetVisitByID(string id) =>
        _dbContext.Visits.Include(v => v.Services).ThenInclude(v => v.Service)
        .Include(v => v.Worker).Include(v => v.Master).Include(v => v.Customer).FirstOrDefault(v => v.ID == id);
        // здесь без фильтрации на актуальность
}
