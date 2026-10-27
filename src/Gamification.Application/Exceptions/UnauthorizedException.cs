namespace Gamification.Application.Exceptions;

/// <summary>
/// Thrown when the apiKey / apiPassword credentials are missing or invalid.
/// Translated by the global exception handling middleware to an HTTP 401 Unauthorized response.
/// </summary>
public class UnauthorizedException : Exception
{
    /// <summary>
    /// Creates a new instance of the exception.
    /// </summary>
    /// <param name="message">The error message.</param>
    public UnauthorizedException(string message) : base(message)
    {
    }
}
