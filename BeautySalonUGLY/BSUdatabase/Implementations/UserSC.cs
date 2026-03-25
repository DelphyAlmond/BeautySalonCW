using BSUcontractmodels.DataModels;
using BSUcontractmodels.StoragesContracts;
using BSUdatabase;
using Microsoft.Extensions.Logging;

namespace BSUdatabase.Implementations;

/// Storage реализация для работы с пользователями в БД
internal class UserSC : IUserSC
{
    private readonly BSUdbContext _dbContext;
    private readonly ILogger<UserSC> _logger;

    public UserSC(BSUdbContext dbContext, ILogger<UserSC> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// Получить пользователя по username
    public UserDM? GetUserByUsername(string username)
    {
        try
        {
            // В реальном проекте это была бы работа с EF Core
            // Пока возвращаем null для примера
            _logger.LogInformation($"Fetching user by username: {username}");
            
            // TODO: Реализовать при наличии DbSet<UserDM> в BSUdbContext
            // var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);
            // return user;
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching user by username: {username}");
            throw;
        }
    }

    /// Получить пользователя по ID
    public UserDM? GetUserById(string userId)
    {
        try
        {
            _logger.LogInformation($"Fetching user by id: {userId}");
            
            // TODO: Реализовать при наличии DbSet<UserDM> в BSUdbContext
            // var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            // return user;
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching user by id: {userId}");
            throw;
        }
    }

    /// Получить всех активных пользователей
    public List<UserDM> GetAllActiveUsers()
    {
        try
        {
            _logger.LogInformation("Fetching all active users");
            
            // TODO: Реализовать при наличии DbSet<UserDM> в BSUdbContext
            // return _dbContext.Users.Where(u => u.IsActive).ToList();
            
            return new List<UserDM>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching active users");
            throw;
        }
    }

    /// Добавить нового пользователя
    public void AddUser(UserDM user)
    {
        try
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            _logger.LogInformation($"Adding new user: {user.Username}");
            
            // TODO: Реализовать при наличии DbSet<UserDM> в BSUdbContext
            // _dbContext.Users.Add(user);
            // _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error adding user: {user?.Username}");
            throw;
        }
    }

    /// Обновить пользователя
    public void UpdateUser(UserDM user)
    {
        try
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            user.UpdatedAt = DateTime.UtcNow;
            _logger.LogInformation($"Updating user: {user.Username}");
            
            // TODO: Реализовать при наличии DbSet<UserDM> в BSUdbContext
            // _dbContext.Users.Update(user);
            // _dbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating user: {user?.Username}");
            throw;
        }
    }

    /// Удалить (деактивировать) пользователя
    public void DeleteUser(string userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentNullException(nameof(userId));

            _logger.LogInformation($"Deactivating user: {userId}");
            
            // TODO: Реализовать при наличии DbSet<UserDM> в BSUdbContext
            // var user = _dbContext.Users.FirstOrDefault(u => u.Id == userId);
            // if (user != null)
            // {
            //     user.IsActive = false;
            //     user.UpdatedAt = DateTime.UtcNow;
            //     _dbContext.Users.Update(user);
            //     _dbContext.SaveChanges();
            // }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting user: {userId}");
            throw;
        }
    }
}
