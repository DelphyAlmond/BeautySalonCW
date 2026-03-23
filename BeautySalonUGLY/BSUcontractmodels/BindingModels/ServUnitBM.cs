using BSUcontrmodels.DataModels;
using System.ComponentModel.DataAnnotations;

namespace BSUcontractmodels.BindingModels;

public class ServUnitBM
{
    // [ ? ] VisitID:
    public string? VisitID { get; set; }

    public string? ServiceID { get; set; }
    public int Count { get; set; }

    public double Price { get; set; }
}
