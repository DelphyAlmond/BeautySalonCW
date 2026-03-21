using AutoMapper;
using BSUcontractmodels.BindingModels;
using BSUcontractmodels.ViewModels;
using BSUcontrmodels.DataModels;

namespace BSUWebAppi.MapProfiles;
public class ServiceProfile : Profile
{
    public ServiceProfile()
    {
        CreateMap<ServiceBM, ServiceDM>();
        CreateMap<ServiceDM, ServiceVM>();
    }
}