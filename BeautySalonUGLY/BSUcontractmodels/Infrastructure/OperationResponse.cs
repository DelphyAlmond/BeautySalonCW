using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BSUcontractmodels.Infrastructure;

// Наследники типизируют Result (список или единичный объект)
// и предоставляют статические фабричные методы.
// Метод GetResponse(унаследованный)
// преобразует объект в IActionResult,
// устанавливая статус‑код и сериализуя Result.

public class OperationResponse
{
    protected HttpStatusCode StatusCode { get; set; }
    protected object? Result { get; set; }

    // Чтобы каждый раз не создавать объект operation response и не прописывать поля
    // можно объявить несколько статичных методов-шаблонов для создания в последующем
    // экземпляров нужных типов под основные задачи (вставка, удаление, изменение):
    // 1. успех\+ нет содержимого
    // 2. непредвиденная ошибка\запись не найдена
    // 3. ошибка сервера

    public IActionResult GetResponse(HttpRequest req, HttpResponse res)
    {
        ArgumentNullException.ThrowIfNull(req);
        ArgumentNullException.ThrowIfNull(res);

        res.StatusCode = (int)StatusCode;

        if (Result is null)
        {
            return new StatusCodeResult((int)StatusCode);
        }

        return new ObjectResult(Result);
    }

    // модификатор protected:    static
    // доступ                    исп. тип,
    // only                      а не конкретный
    // для наследников ⤵         экземпляр ⤵
    protected static TResult OK<TResult, TData>(TData data) where TResult : OperationResponse, new() =>
        new() {
            StatusCode = HttpStatusCode.OK,
            Result = data
        };

    protected static TResult NoContent<TResult>() where TResult : OperationResponse, new() =>
        new() { StatusCode = HttpStatusCode.NoContent };

    protected static TResult BadRequest<TResult>(string? errorMessage = null) where TResult :
        OperationResponse, new() =>
        new() {
            StatusCode =
        HttpStatusCode.BadRequest,
            Result = errorMessage
        };

    protected static TResult NotFound<TResult>(string? errorMessage = null) where TResult :
        OperationResponse, new() =>
        new()
        {
            StatusCode =
        HttpStatusCode.NotFound,
            Result = errorMessage
        };

    protected static TResult InternalServerError<TResult>(string? errorMessage = null)
        where TResult : OperationResponse, new() =>
        new()
        {
            StatusCode =
        HttpStatusCode.InternalServerError,
            Result = errorMessage
        };
}
