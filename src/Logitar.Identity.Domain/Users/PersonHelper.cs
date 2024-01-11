namespace Logitar.Identity.Domain.Users;

/// <summary>
/// Defines methods to help handle persons.
/// </summary>
public static class PersonHelper
{
  /// <summary>
  /// Builds the full name of a person from its specified list of names.
  /// </summary>
  /// <param name="names">The list of names.</param>
  /// <returns>The full name.</returns>
  public static string? BuildFullName(params PersonNameUnit?[] names) => BuildFullName(names.Select(name => name?.Value).ToArray());
  /// <summary>
  /// Builds the full name of a person from its specified list of names.
  /// </summary>
  /// <param name="names">The list of names.</param>
  /// <returns>The full name.</returns>
  public static string? BuildFullName(params string?[] names) => string.Join(' ', names
    .SelectMany(name => name?.Split(' ') ?? [])
    .Where(name => !string.IsNullOrEmpty(name))).CleanTrim();

}
