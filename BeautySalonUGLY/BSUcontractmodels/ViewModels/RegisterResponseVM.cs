namespace BSUcontractmodels.ViewModels;

/// ViewModel для запроса регистрации
public class RegisterRequestVM
{
    /// Имя пользователя (логин)
    public required string Username { get; set; }

    /// Email пользователя
    public required string Email { get; set; }

    /// ФИ пользователя
    public required string FullName { get; set; }

    /// Пароль
    public required string Password { get; set; }
}

/// ViewModel для ответа при успешной регистрации
public class RegisterResponseVM
{
    /// Уникальный ID новго пользователя
    public required string Id { get; set; }

    /// Сообщение об успешной регистрации
    public required string Message { get; set; }
}
