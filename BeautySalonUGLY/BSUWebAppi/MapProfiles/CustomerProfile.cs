using AutoMapper;
using BSUcontractmodels.BindingModels;
using BSUcontractmodels.ViewModels;
using BSUcontrmodels.DataModels;

namespace BSUWebAppi.MapProfiles;

public class CustomerProfile : Profile
{
    public CustomerProfile()
    {
        CreateMap<CustomerBM, CustomerDM>();
        CreateMap<CustomerDM, CustomerVM>();
    }
}
