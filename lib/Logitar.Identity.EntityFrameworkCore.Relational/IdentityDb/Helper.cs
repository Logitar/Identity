namespace Logitar.Identity.EntityFrameworkCore.Relational.IdentityDb;

public static class Helper
{
  public static string Normalize(string value) => value.Trim().ToUpperInvariant();
}
