using AutoMapper;
using BSUcontractmodels.BindingModels;
using BSUcontractmodels.ViewModels;
using BSUcontrmodels.DataModels;

namespace BSUWebAppi.MapProfiles;

public class WorkerProfile : Profile
{
    public WorkerProfile()
    {
        CreateMap<WorkerBM, WorkerDM>();
        CreateMap<WorkerDM, WorkerVM>();
    }
}
