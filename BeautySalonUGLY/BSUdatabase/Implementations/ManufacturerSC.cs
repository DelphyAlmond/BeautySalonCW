using AutoMapper;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUdatabase.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;

namespace BSUdatabase.Implementations;

internal class ManufacturerSC : IManufacturerSC
{
    private readonly BSUdbContext _dbContext;
    private readonly Mapper _mapper;

    public ManufacturerSC(BSUdbContext context)
    {
        _dbContext = context;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Manufacturer, ManufacturerDM>();
            cfg.CreateMap<ManufacturerDM, Manufacturer>()
                .ForMember(dest => dest.CurrentName, opt => opt.MapFrom(src => src.Manufacturer));
        }, NullLoggerFactory.Instance);
        _mapper = new Mapper(config);
    }

    public List<ManufacturerDM> GetManufacturers()
    {
        try
        {
            return [.. _dbContext.Manufacturers.Select(x => _mapper.Map<ManufacturerDM>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ManufacturerDM? GetMByID(string id)
    {
        try
        {
            return _mapper.Map<ManufacturerDM>(GetManufacturerByID(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ManufacturerDM? GetMByName(string naming)
    {
        try
        {
            return _mapper.Map<ManufacturerDM>(
                _dbContext.Manufacturers.FirstOrDefault(x => x.CurrentName == naming));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public ManufacturerDM? GetMByPrevName(string naming)
    {
        try
        {
            return _mapper.Map<ManufacturerDM>(
                _dbContext.Manufacturers.FirstOrDefault(x => x.LastPrevNaming == naming
                || x.SecondPrevNaming == naming));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddM(ManufacturerDM manufacturer)
    {
        try
        {
            _dbContext.Manufacturers.Add(_mapper.Map<Manufacturer>(manufacturer));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", manufacturer.ID);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Manufacturers_CurrentName" })
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("ManufacturerName", manufacturer.Manufacturer);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdM(ManufacturerDM manufacturer)
    {
        try
        {
            var entity = GetManufacturerByID(manufacturer.ID
                ?? throw new Exception($"< * > ObjManuf-r not found: {manufacturer.Manufacturer}"));
            if (entity.CurrentName != manufacturer.Manufacturer)
            {
                entity.LastPrevNaming = entity.SecondPrevNaming;
                entity.SecondPrevNaming = entity.CurrentName;
                entity.CurrentName = manufacturer.Manufacturer;
            }
            // _mapper.Map(manufacturer, entity);
            _dbContext.Manufacturers.Update(entity);
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Manufacturers_CurrentName" })
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("ManufacturerName", manufacturer.Manufacturer);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void DelM(string id)
    {
        try
        {
            var entity = GetManufacturerByID(id) ?? throw new Exception($"< * > ObjManuf-r not found: {id}");
            _dbContext.Manufacturers.Remove(entity);
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Manufacturer? GetManufacturerByID(string id) =>
        _dbContext.Manufacturers.FirstOrDefault(x => x.ID == id);
}