using BSUcontractmodels.BusinessLogicContracts;
using BSUcontractmodels.DataModels;
using BSUcontractmodels.StoragesContracts;
using BSUmodels.Extensions;
using BSUmodels.Exceptions;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace BSUbusinesslogic.Implementations;

/// Реализация сервиса аутентификации
/// Отвечает за проверку учетных данных и управление пользователями
public class AuthenticationBLC : IAuthenticationBLC
{
    private readonly IUserSC _userSC;
    private readonly ILogger<AuthenticationBLC> _logger;

    public AuthenticationBLC(IUserSC userSC, ILogger<AuthenticationBLC> logger)
    {
        _userSC = userSC ?? throw new ArgumentNullException(nameof(userSC));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// Аутентифицировать пользователя по username и пароль
    public async Task<UserDM?> AuthenticateAsync(string username, string password)
    {
        // Валидация входных данных
        if (username.IsEmpty() || password.IsEmpty())
        {
            _logger.LogWarning("Authentication attempt with empty username or password");
            return null;
        }

        // Получаем пользователя из БД
        var user = await Task.Run(() => _userSC.GetUserByUsername(username));

        if (user == null)
        {
            _logger.LogWarning($"Authentication failed: User '{username}' not found");
            return null;
        }

        // Проверяем, активен ли пользователь
        if (!user.IsActive)
        {
            _logger.LogWarning($"Authentication failed: User '{username}' is inactive");
            return null;
        }

        // Проверяем пароль (используем BCrypt для безопасного сравнения)
        if (!VerifyPassword(password, user.PasswordHash))
        {
            _logger.LogWarning($"Authentication failed: Invalid password for user '{username}'");
            return null;
        }

        _logger.LogInformation($"User '{username}' authenticated successfully");
        return user;
    }

    /// Получить пользователя по username
    public async Task<UserDM?> GetUserByUsernameAsync(string username)
    {
        if (username.IsEmpty())
            throw new ValidationException("< Auth BLC: Username is empty >");

        return await Task.Run(() => _userSC.GetUserByUsername(username));
    }

    /// Получить пользователя по ID
    public async Task<UserDM?> GetUserByIdAsync(string userId)
    {
        if (userId.IsEmpty() || !userId.IsGuid())
            throw new ValidationException("< Auth BLC: Invalid user ID >");

        return await Task.Run(() => _userSC.GetUserById(userId));
    }

    /// Зарегистрировать нового пользователя
    public async Task<UserDM> RegisterUserAsync(string username, string email, string fullName, string password, string role = "User")
    {
        // Валидация входных данных
        if (username.IsEmpty() || email.IsEmpty() || fullName.IsEmpty() || password.IsEmpty())
            throw new ValidationException("< Auth BLC: All fields are required >");

        // Проверяем, не существует ли уже такой пользователь
        var existingUser = await GetUserByUsernameAsync(username);
        if (existingUser != null)
            throw new ValidationException($"< Auth BLC: User '{username}' already exists >");

        // Создаем нового пользователя
        var newUser = new UserDM
        {
            Username = username,
            Email = email,
            FullName = fullName,
            PasswordHash = HashPassword(password),
            Role = role,
            IsActive = true
        };

        await Task.Run(() => _userSC.AddUser(newUser));
        _logger.LogInformation($"User '{username}' registered successfully with role '{role}'");

        return newUser;
    }

    /// Проверить, активен ли пользователь
    public async Task<bool> IsUserActiveAsync(string userId)
    {
        if (userId.IsEmpty())
            return false;

        var user = await GetUserByIdAsync(userId);
        return user?.IsActive ?? false;
    }

     /// Хешировать пароль (используем PBKDF2 для безопасности)
     private static string HashPassword(string password)
     {
         // Используем PBKDF2 для безопасного хеширования с солью
         using (var rfc2898 = new Rfc2898DeriveBytes(password, 16, 10000, HashAlgorithmName.SHA256))
         {
             byte[] hash = rfc2898.GetBytes(20);
             byte[] salt = rfc2898.Salt;

             // Комбинируем соль и хеш
             byte[] hashWithSalt = new byte[36];
             Array.Copy(salt, 0, hashWithSalt, 0, 16);
             Array.Copy(hash, 0, hashWithSalt, 16, 20);

             // Кодируем в Base64 для хранения
             return Convert.ToBase64String(hashWithSalt);
         }
     }

     /// Проверить пароль против хеша
     private static bool VerifyPassword(string password, string hash)
     {
         try
         {
             // Декодируем Base64
             byte[] hashWithSalt = Convert.FromBase64String(hash);

             // Извлекаем соль
             byte[] salt = new byte[16];
             Array.Copy(hashWithSalt, 0, salt, 0, 16);

             // Хешируем введенный пароль с той же солью
             using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
             {
                 byte[] newHash = rfc2898.GetBytes(20);

                 // Сравниваем хеши
                 for (int i = 0; i < 20; i++)
                 {
                     if (hashWithSalt[i + 16] != newHash[i])
                         return false;
                 }

                 return true;
             }
         }
         catch
         {
             return false;
         }
    }
}
