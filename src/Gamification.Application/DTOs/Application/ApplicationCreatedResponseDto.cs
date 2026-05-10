namespace Gamification.Application.DTOs.Application;

/// <summary>
/// Represents the response returned after creating an application.
/// </summary>
public class ApplicationCreatedResponseDto
{
    /// <summary>
    /// Status of the creation operation.
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// URL of the newly created application resource.
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// Unique identifier of the created application.
    /// </summary>
    public int Id { get; set; }
}
