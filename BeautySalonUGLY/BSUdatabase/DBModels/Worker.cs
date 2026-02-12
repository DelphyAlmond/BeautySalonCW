using BSUcontrmodels.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace BSUdatabase.DBModels;

internal class Worker
{
    public string ID { get; set; }
    public string FullName { get; set; }
    public Post PostType { get; set; }
    public string Password { get; set; }
    public bool IsDeleted { get; set; }

    public DateTime BirthDate { get; set; }
    public DateTime EmploymentDate { get; set; }

    // --------------------------------------------- *

    [ForeignKey("WorkerID")]
    public List<Order>? Orders { get; set; }

    [ForeignKey("MasterID")]
    public List<Order>? OrdersAsMaster { get; set; }
    //          ^- if master - linked by

    [ForeignKey("WorkerID")]
    public List<Visit>? Visits { get; set; }
}
