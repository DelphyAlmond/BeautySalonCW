using BSUcontrmodels.DataModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BSUcontractmodels.BindingModels;

public class VisitBM
{
    public string? ID { get; set; }
    public string? CustomerID { get; set; }
    public string? OrderID { get; set; }
    public string? WorkerID { get; set; }
    public string? MasterID { get; set; }

    // * left inside the system: public DateTime [ DateReg & DatePlanned ] { get; set; }
    public List<ServUnitBM>? Services { get; set; } // ← Binding Model

    public double Discount { get; set; }
    public bool IsCanceled { get; set; }
}
