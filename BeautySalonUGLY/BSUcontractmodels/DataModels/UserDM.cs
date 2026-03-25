namespace BSUcontractmodels.DataModels;

/// Data Model для пользователя системы
public class UserDM
{
    /// Уникальный идентификатор пользователя
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// Имя пользователя (логин)
    public required string Username { get; set; }

    /// Email пользователя
    public required string Email { get; set; }

    /// ФИ пользователя
    public required string FullName { get; set; }

    /// Хешированный пароль (никогда не храним пароль в открытом виде!)
    public required string PasswordHash { get; set; }

    /// Роль пользователя (Manager(Worker), User(Customer))
    public required string Role { get; set; }

    /// Активен ли пользователь
    public bool IsActive { get; set; } = true;

    /// Дата создания учетной записи
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// Дата последнего обновления
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
