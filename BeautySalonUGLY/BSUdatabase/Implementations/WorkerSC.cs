using AutoMapper;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUdatabase;
using BSUdatabase.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;

internal class WorkerSC : IWorkerSC
{
    private readonly BSUdbContext _dbContext;
    private readonly Mapper _mapper;

    public WorkerSC(BSUdbContext context)
    {
        _dbContext = context;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Worker, WorkerDM>();
            cfg.CreateMap<WorkerDM, Worker>();
        }, NullLoggerFactory.Instance);
        _mapper = new Mapper(config);
    }

    public List<WorkerDM> GetWorkers(bool onlyActive = true,
        DateTime? fromBD = null, DateTime? toBD = null,
        DateTime? fromEmploymentDate = null,
        DateTime? toEmploymentDate = null)
    {
        try
        {
            var query = _dbContext.Workers.AsQueryable();
            if (onlyActive) query = query.Where(w => !w.IsDeleted);
            if (fromBD is not null && toBD is not null)
                query = query.Where(w => w.BirthDate >= fromBD && w.BirthDate <= toBD);
            if (fromEmploymentDate is not null && toEmploymentDate is not null)
                query = query.Where(w => w.EmploymentDate >= fromEmploymentDate
                && w.EmploymentDate <= toEmploymentDate);

            return [.. query.Select(x => _mapper.Map<WorkerDM>(x))];
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public WorkerDM? GetWByID(string id)
    {
        try
        {
            return _mapper.Map<WorkerDM>(GetWorkerByID(id));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public WorkerDM? GetWByName(string fullname)
    {
        try
        {
            return _mapper.Map<WorkerDM>(_dbContext.Workers.FirstOrDefault(w => w.FullName == fullname));
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddW(WorkerDM worker)
    {
        try
        {
            _dbContext.Workers.Add(_mapper.Map<Worker>(worker));
            _dbContext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", worker.ID);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Workers_FullName" })
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("FullName", worker.FullName);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdW(WorkerDM worker)
    {
        try
        {
            var entity = GetWorkerByID(worker.ID
                ?? throw new Exception($"< * > ObjWorker not found: {worker.FullName}; ID: {worker.ID}"));
            _dbContext.Workers.Update(_mapper.Map(worker, entity));
            _dbContext.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Workers_FullName" })
        {
            _dbContext.ChangeTracker.Clear();
            throw new ElementExistsException("FullName", worker.FullName);
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void DelW(string id)
    {
        try
        {
            var entity = GetWorkerByID(id) ?? throw new Exception($"< * > ObjWorker not found: {id}");
            entity.IsDeleted = true; // [ ! ] мягкое удаление
            _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _dbContext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    private Worker? GetWorkerByID(string id) =>
        _dbContext.Workers.FirstOrDefault(w => w.ID == id && !w.IsDeleted);
}
