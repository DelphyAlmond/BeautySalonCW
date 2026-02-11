namespace BSUmodels.Extensions;
// Статичный класс-расширение для методов проверки строчных шаблонов
public static class StrExtensions
{
    // 2 начальн. исключения:
    public static bool IsEmpty(this string str)
    {
        return string.IsNullOrWhiteSpace(str);
    }
    public static bool IsGuid(this string idStr)
    {
        return Guid.TryParse(idStr, out _);
    }
}
