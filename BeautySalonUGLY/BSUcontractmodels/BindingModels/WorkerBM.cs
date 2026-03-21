using BSUcontrmodels.Enums;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace BSUcontractmodels.BindingModels;

public class WorkerBM
{
    [Required(ErrorMessage = "> ID обязателен")]
    public string? ID { get; set; }
    public string? FullName { get; set; }
    public Post PostType { get; set; }
    public string? Password { get; set; }
    public bool IsDeleted { get; set; }

    public DateTime BirthDate { get; set; }
    public DateTime EmploymentDate { get; set; }
}
