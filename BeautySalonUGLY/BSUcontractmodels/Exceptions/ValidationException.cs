namespace BSUmodels.Exceptions;
// В случае неудачной проверки\неуспешной валидации - выброс персонализированной\кастомной ошибки
public class ValidationException(string message) : Exception(message) // публичн. - т.к выбрасывать и ловить придётся и в др. проектах
{
    // >> вместо public ValidationException(string message) : base(message) { }
}
