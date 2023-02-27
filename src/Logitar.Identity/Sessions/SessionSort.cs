namespace Logitar.Identity.Sessions;

/// <summary>
/// Represents the possible sort values for user sessions.
/// </summary>
public enum SessionSort
{
  /// <summary>
  /// The user sessions will be sorted by their signing-out date and time.
  /// </summary>
  SignedOutOn,

  /// <summary>
  /// The user sessions will be sorted by their latest update date and time, including creation.
  /// </summary>
  UpdatedOn
}
