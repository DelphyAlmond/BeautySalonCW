using System.ComponentModel.DataAnnotations;

namespace BSUcontractmodels.BindingModels;

public class ManufacturerBM
{
    [Required(ErrorMessage = "> ID обязателен")]
    public string? ID { get; set; }
    public string? Manufacturer { get; set; }
    public string? LastPrevNaming { get; set; }
    public string? SecondPrevNaming { get; set; }
}
