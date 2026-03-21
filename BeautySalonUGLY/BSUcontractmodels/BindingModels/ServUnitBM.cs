using BSUcontrmodels.DataModels;
using System.ComponentModel.DataAnnotations;

namespace BSUcontractmodels.BindingModels;

public class ServUnitBM
{
    // [ x ] VisitID
    public string? ServiceID { get; set; }
    public int Count { get; set; }
}
