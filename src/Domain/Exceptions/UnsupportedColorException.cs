namespace Domain.Exceptions;

public class UnsupportedColorException(string code) : Exception($"Color \"{code}\" is unsupported.")
{
}
