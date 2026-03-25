using BSUcontractmodels.DataModels;

namespace BSUcontractmodels.StoragesContracts;

/// Storage контракт для работы с пользователями в БД
public interface IUserSC
{
    /// Получить пользователя по username
    UserDM? GetUserByUsername(string username);

    /// Получить пользователя по ID
    UserDM? GetUserById(string userId);

    /// Получить всех активных пользователей
    List<UserDM> GetAllActiveUsers();

    /// Добавить нового пользователя
    void AddUser(UserDM user);

    /// Обновить пользователя
    void UpdateUser(UserDM user);

    /// Удалить пользователя (деактивировать)
    void DeleteUser(string userId);
}
