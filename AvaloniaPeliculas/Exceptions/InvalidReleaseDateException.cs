using System;

namespace AvaloniaPeliculas.Exceptions;

public class InvalidReleaseDateException : Exception
{
    public InvalidReleaseDateException(string message) : base(message)
    {
    }
}