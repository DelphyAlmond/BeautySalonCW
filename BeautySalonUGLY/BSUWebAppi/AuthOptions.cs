using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BSUWebAppi;

// 1. этап настройки аутентификации. Подход Bearer
// (Пр.-а после успешной проверки выдаст токен и будет проверять его при каждом повторном запросе к сервису)*

public class AuthOptions
{
    public const string ISSUER = "BSU_AuthServer"; // Издатель-выдающий токена
    public const string AUDIENCE = "BSU_Client"; // Потребитель
    const string KEY = "beautysalonbeugly_secretkey!73"; // ключ для шифрования

    public static SymmetricSecurityKey GetSymmetricSecurityKey() => new(Encoding.UTF8.GetBytes(KEY));
}
