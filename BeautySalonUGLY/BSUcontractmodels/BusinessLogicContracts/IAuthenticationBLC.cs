using BSUcontractmodels.DataModels;

namespace BSUcontractmodels.BusinessLogicContracts;

/// Контракт для сервиса аутентификации
/// Отвечает за логику проверки пользователей и генерацию JWT токенов
public interface IAuthenticationBLC
{
    /// Проверить учетные данные пользователя (username и пароль)
    /// Имя пользователя + Пароль в открытом виде => Данные пользователя если аутентификация успешна, иначе null
    Task<UserDM?> AuthenticateAsync(string username, string password);

    /// Получить пользователя по username
    Task<UserDM?> GetUserByUsernameAsync(string username);

    /// Получить пользователя по ID
    Task<UserDM?> GetUserByIdAsync(string userId);

    /// Зарегистрировать нового пользователя
    Task<UserDM> RegisterUserAsync(string username, string email, string fullName, string password, string role = "User");

    /// Проверить, активен ли пользователь
    Task<bool> IsUserActiveAsync(string userId);
}
