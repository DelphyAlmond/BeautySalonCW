namespace BSUWebAppi.InfrastructureDB;

using BSUcontractmodels.Infrastructure;
using Microsoft.Extensions.Configuration;

// Для того, чтоб достать с appsettings строку подключения понадобится:
// встроить интерфейс IConfiguration
// и прописать поле, типа лейзи (lazy - для ресурсозатратных операций), у которого в последствии вызвется объект-метод value,
// при обращении, и ТОЛЬКО при обр. к которому он будет единожды воспроизводить функцию и послед. запомнит и выдаст результат.

public class ConfigurationDB(IConfiguration conf) : IConfigurationDatabase
{
    private Lazy<DBSettings> _dbSettings => new(() =>
    {
        return conf.GetValue<DBSettings>("DBSettings") ?? throw new InvalidDataException(nameof(DBSettings));
    });
    public string ConnectionStr => _dbSettings.Value.ConnectionString;
}
