namespace BSUcontractmodels.ViewModels;

/// ViewModel для запроса аутентификации (логина)
public class LoginRequestVM
{
    /// Имя пользователя
    public required string Username { get; set; }

    /// Пароль
    public required string Password { get; set; }
}

/// ViewModel для ответа с JWT токеном
public class LoginResponseVM
{
    /// JWT токен
    public required string Token { get; set; }

    /// Время жизни токена в минутах
    public int ExpiresIn { get; set; } = 480; // 8 часов

    /// Информация о пользователе, который залогинился
    public required UserInfoVM User { get; set; }
}

/// ViewModel с информацией о пользователе
public class UserInfoVM
{
    public required string Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string FullName { get; set; }
    public required string Role { get; set; }
}
