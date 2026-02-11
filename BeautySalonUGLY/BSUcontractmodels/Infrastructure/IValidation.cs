namespace BSUcontrmodels.Infrastructure;
// Родительский класс всех моделей, для реализации метода проверки присваеваемых (в поля) входных данных
// (как минимальная начальн.валидация)
public interface IValidation
{
    void Validate();
}
