using BSUcontrmodels.Enums;

namespace BSUcontractmodels.ViewModels;

public class WorkerVM
{
    public required string ID { get; set; }
    public required string FullName { get; set; }
    public required Post PostType { get; set; }
    public required bool IsDeleted { get; set; }
    public required DateTime BirthDate { get; set; }
    public required DateTime EmploymentDate { get; set; }
    // > пароль не возвращается клиенту по соображениям безопасности -
    // т.е Password не включается в View Model,
    // чтобы случайно не отправить хеш или сам пароль клиенту.
}