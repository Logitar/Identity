namespace Logitar.Identity.Domain.Users;

public static class UserHelper
{
  public static string? BuildFullName(params PersonNameUnit?[] names)
    => BuildFullName(names.Select(name => name?.Value).ToArray());
  public static string? BuildFullName(params string?[] names) => string.Join(' ', names
    .SelectMany(name => name?.Split(' ') ?? [])
    .Where(name => !string.IsNullOrEmpty(name))).CleanTrim();
}
