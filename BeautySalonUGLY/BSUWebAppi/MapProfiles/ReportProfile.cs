using AutoMapper;
using BSUcontractmodels.DataModels;
using BSUcontractmodels.ViewModels;

namespace BSUWebAppi.MapProfiles;

/// Профиль маппинга для отчётов по посещениям
/// Преобразует MasterVisitsDM в MasterVisitsVM для передачи клиенту
public class ReportProfile : Profile
{
    public ReportProfile()
    {
        // Маппинг MasterVisitsDM -> MasterVisitsVM
        CreateMap<MasterVisitsDM, MasterVisitsVM>()
            .ForMember(dest => dest.MasterFullName, opt => opt.MapFrom(src => src.MasterFullName))
            .ForMember(dest => dest.Visits, opt => opt.MapFrom(src => src.Visits));
    }
}
