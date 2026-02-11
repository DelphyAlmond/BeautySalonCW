namespace BSUcontractmodels.Exceptions;

public class ElementNotFoundException : Exception
{
    public ElementNotFoundException(string? message, string obj) : base(message ?? "" + $">> The element '{obj}' doesn't exist") { }
}
