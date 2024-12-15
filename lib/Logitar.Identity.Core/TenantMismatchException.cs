namespace Logitar.Identity.Core;

/// <summary>
/// The exception raised when an association is made between two entities in different tenants.
/// </summary>
public class TenantMismatchException : Exception
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  private const string ErrorMessage = "The specified tenant identifier was not expected.";

  /// <summary>
  /// Gets or sets the expected tenant identifier.
  /// </summary>
  public string? ExpectedTenantId
  {
    get => (string?)Data[nameof(ExpectedTenantId)];
    private set => Data[nameof(ExpectedTenantId)] = value;
  }
  /// <summary>
  /// Gets or sets the actual tenant identifier.
  /// </summary>
  public string? ActualTenantId
  {
    get => (string?)Data[nameof(ActualTenantId)];
    private set => Data[nameof(ActualTenantId)] = value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="TenantMismatchException"/> class.
  /// </summary>
  /// <param name="expected">The expected tenant identifier.</param>
  /// <param name="actual">The actual tenant identifier.</param>
  public TenantMismatchException(TenantId? expected, TenantId? actual)
    : base(BuildMessage(expected, actual))
  {
    ExpectedTenantId = expected?.Value;
    ActualTenantId = actual?.Value;
  }

  /// <summary>
  /// Builds the exception message.
  /// </summary>
  /// <param name="expected">The expected tenant identifier.</param>
  /// <param name="actual">The actual tenant identifier.</param>
  /// <returns>The exception message.</returns>
  private static string BuildMessage(TenantId? expected, TenantId? actual) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ExpectedTenantId), expected, "<null>")
    .AddData(nameof(ActualTenantId), actual, "<null>")
    .Build();
}
