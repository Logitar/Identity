namespace Logitar.Identity.Sessions;

/// <summary>
/// The base session update input data.
/// </summary>
public abstract record SaveSessionInput
{
  /// <summary>
  /// Gets or sets the custom attributes of the user session.
  /// </summary>
  public IEnumerable<CustomAttribute>? CustomAttributes { get; set; }
}
