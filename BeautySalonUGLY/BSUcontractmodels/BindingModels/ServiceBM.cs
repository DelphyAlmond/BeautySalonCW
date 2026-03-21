using System.ComponentModel.DataAnnotations;

namespace BSUcontractmodels.BindingModels;

public class ServiceBM
{
    [Required(ErrorMessage = "> ID обязателен")]
    public string? ID { get; set; }
    public string? ServiceNaming { get; set; }
    public double Price { get; set; }
    public string? Description { get; set; }
    public int Duration { get; set; }
    public bool IsDeleted { get; set; }
}
