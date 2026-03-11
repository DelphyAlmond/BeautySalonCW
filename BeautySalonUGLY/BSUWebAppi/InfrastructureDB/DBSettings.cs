namespace BSUWebAppi.InfrastructureDB;

// 2. этап настройки по подключению к БД будут вноситься в первую очередь в appsettings.json,
// чтобы без глобальных изм. можно было бы перепрыгнуть на др. БД. (не пересобирая программу)

// Затем, чтобы вытягивать её оттуда понадобится 2 класса:
// 1) описание config и 2) реализация имеющегося в предыдущ. проекте ConnectionString

// appsettings.json: +
// "DBSettings" : { - назв. именование класса
//   "Connectiontring" : "" - его элемент-поле
// }

public class DBSettings
{
    public required string ConnectionString { get; set; }
}
