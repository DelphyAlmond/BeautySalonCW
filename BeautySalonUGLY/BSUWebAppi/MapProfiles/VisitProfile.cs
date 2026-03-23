using AutoMapper;
using BSUcontractmodels.BindingModels;
using BSUcontractmodels.ViewModels;
using BSUcontrmodels.DataModels;

namespace BSUWebAppi.MapProfiles;

public class VisitProfile : Profile
{
    public VisitProfile()
    {
        CreateMap<ServUnitBM, ServUnitVisitLinkDM>()
            .ForMember(dest => dest.VisitID, opt => opt.Ignore())
            .ReverseMap();

        CreateMap<VisitBM, VisitDM>()
            .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Services));

        CreateMap<ServUnitVisitLinkDM, ServUnitVisitLinkVM>()
            .ForMember(dest => dest.ServiceName, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.Ignore());

        CreateMap<VisitDM, VisitVM>()
            .ForMember(dest => dest.Services, opt => opt.MapFrom(src => src.Services))
            .ForMember(dest => dest.CustomerName, opt => opt.Ignore())
            .ForMember(dest => dest.WorkerName, opt => opt.Ignore())
            .ForMember(dest => dest.MasterName, opt => opt.Ignore()); // < [ + ] *
    }
}