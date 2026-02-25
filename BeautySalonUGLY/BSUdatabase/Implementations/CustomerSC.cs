using AutoMapper;
using BSUcontractmodels.StoragesContracts;
using BSUcontrmodels.DataModels;
using BSUdatabase.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Npgsql;

namespace BSUdatabase.Implementations;

internal class CustomerSC : ICustomerSC
{
    private readonly BSUdbContext _salonDBcontext;
    private readonly Mapper _mapper;

    public CustomerSC(BSUdbContext bsDBc)
    {
        _salonDBcontext = bsDBc;
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Customer, CustomerDM>();
            cfg.CreateMap<CustomerDM, Customer>();
        }, NullLoggerFactory.Instance);

        _mapper = new Mapper(config);
    }

    public List<CustomerDM> GetCustomers()
    {
        try
        {
            return [.._salonDBcontext.Cusromers.Select(x => _mapper.Map<CustomerDM>(x))];
        }
        catch (Exception ex)
        {
            _salonDBcontext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public CustomerDM? GetCByID(string id)
    {
        try
        {
            return _mapper.Map<CustomerDM>(GetCustomerByID(id));
        }
        catch (Exception ex)
        {
            _salonDBcontext.ChangeTracker.Clear();
            throw new StorageException(ex); // [ custom exception in BSUcontractmodels ] *
        }
    }

    public CustomerDM? GetCByName(string username)
    {
        try
        {
            return _mapper.Map<CustomerDM>(_salonDBcontext.Cusromers.FirstOrDefault(x => x.Username == username));
        }
        catch (Exception ex)
        {
            _salonDBcontext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public CustomerDM? GetCByPhone(string phoneNumber)
    {
        try
        {
            return _mapper.Map<CustomerDM>(
                _salonDBcontext.Cusromers.FirstOrDefault(x => x.Phonenumber == phoneNumber));
        }
        catch (Exception ex)
        {
            _salonDBcontext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void AddC(CustomerDM customer)
    {
        try
        {
            _salonDBcontext.Cusromers.Add(_mapper.Map<Customer>(customer));
            _salonDBcontext.SaveChanges();
        }
        catch (InvalidOperationException ex) when (ex.TargetSite?.Name == "ThrowIdentityConflict")
        {
            _salonDBcontext.ChangeTracker.Clear();
            throw new ElementExistsException("Id", customer.ID);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Customers_PhoneNumber" })
        {
            _salonDBcontext.ChangeTracker.Clear();
            throw new ElementExistsException("PhoneNumber", customer.Phonenumber);
        }
        catch (Exception ex)
        {
            _salonDBcontext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }

    public void UpdC(CustomerDM customer)
    {
        try
        {
            var entityObjElement = GetCustomerByID(customer.ID
                ?? throw new Exception($"< * > ObjCustomer not found: {customer.ID}"));
            _salonDBcontext.Cusromers.Update(_mapper.Map(customer, entityObjElement));
            _salonDBcontext.SaveChanges();
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException { ConstraintName: "IX_Customers_PhoneNumber" })
        {
            _salonDBcontext.ChangeTracker.Clear();
            throw new ElementExistsException("PhoneNumber", customer.Phonenumber);
        }
        catch (Exception ex)
        {
            _salonDBcontext.ChangeTracker.Clear();
            throw new StorageException(ex);
        }
    }
    
    public void DelC(string id)
    {
        try
        {
            var entityObjElement = GetCustomerByID(id) ?? throw new Exception($"< * > ObjCustomer not found: {id}");
            _salonDBcontext.Cusromers.Remove(entityObjElement);
            _salonDBcontext.SaveChanges();
        }
        catch (Exception ex)
        {
            _salonDBcontext.ChangeTracker.Clear();
            throw new StorageException(ex);    
        }
    }

    private Customer? GetCustomerByID(string id) =>
        _salonDBcontext.Cusromers.FirstOrDefault(x => x.ID == id);
}
