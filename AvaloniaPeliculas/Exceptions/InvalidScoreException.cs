using System;

namespace AvaloniaPeliculas.Exceptions;

public class InvalidScoreException : Exception
{
    public InvalidScoreException(string message) : base(message) { }
}