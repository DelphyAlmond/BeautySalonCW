using AutoMapper;
using BSUcontractmodels.BindingModels;
using BSUcontractmodels.ViewModels;
using BSUcontrmodels.DataModels;

namespace BSUWebAppi.MapProfiles;

public class ManufacturerProfile : Profile
{
    public ManufacturerProfile()
    {
        CreateMap<ManufacturerBM, ManufacturerDM>();
        CreateMap<ManufacturerDM, ManufacturerVM>();
    }
}
