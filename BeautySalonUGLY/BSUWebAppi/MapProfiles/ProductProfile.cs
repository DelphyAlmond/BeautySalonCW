using AutoMapper;
using BSUcontractmodels.BindingModels;
using BSUcontractmodels.ViewModels;
using BSUcontrmodels.DataModels;

namespace BSUWebAppi.MapProfiles;
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductBM, ProductDM>();
        CreateMap<ProductDM, ProductVM>();
    }
}
