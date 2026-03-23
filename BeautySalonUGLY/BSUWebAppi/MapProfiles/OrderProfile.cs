using AutoMapper;
using BSUcontractmodels.BindingModels;
using BSUcontractmodels.ViewModels;
using BSUcontrmodels.DataModels;

namespace BSUWebAppi.MapProfiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        // [ ! ] Преобразование списка ProdUnitBM в список ProdUnitOrderLinkDM
        CreateMap<ProdUnitBM, ProdUnitOrderLinkDM>()
            .ForMember(dest => dest.OrderID, opt => opt.Ignore()) // OrderID будет установлен позже *
            .ReverseMap();

        CreateMap<OrderBM, OrderDM>()
            .ForMember(dest => dest.Cart, opt => opt.MapFrom(src => src.Cart));

        // > Далее по цепочки также из списка ProdUnitOrderLinkDM в список ProdUnitOrderLinkVM
        CreateMap<ProdUnitOrderLinkDM, ProdUnitOrderLinkVM>()
            .ForMember(dest => dest.ProductName, opt => opt.Ignore()) // имя подтягивается отдельно *
            .ForMember(dest => dest.Price, opt => opt.Ignore());

        CreateMap<OrderDM, OrderVM>()
            .ForMember(dest => dest.Cart, opt => opt.MapFrom(src => src.Cart))
            .ForMember(dest => dest.CustomerName, opt => opt.Ignore())
            .ForMember(dest => dest.WorkerName, opt => opt.Ignore());
    }
}
