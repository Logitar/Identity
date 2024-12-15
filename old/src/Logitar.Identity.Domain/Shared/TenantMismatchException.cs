namespace Logitar.Identity.Domain.Shared;

/// <summary>
/// The exception raised when an association between two entities in different tenants is made.
/// </summary>
public class TenantMismatchException : Exception
{
  /// <summary>
  /// A generic error message for this exception.
  /// </summary>
  public const string ErrorMessage = "The specified tenant identifier was not expected.";

  /// <summary>
  /// Gets or sets the expected tenant identifier.
  /// </summary>
  public TenantId? ExpectedTenantId
  {
    get => TenantId.TryCreate((string?)Data[nameof(ExpectedTenantId)]);
    private set => Data[nameof(ExpectedTenantId)] = value?.Value;
  }
  /// <summary>
  /// Gets or sets the actual tenant identifier.
  /// </summary>
  public TenantId? ActualTenantId
  {
    get => TenantId.TryCreate((string?)Data[nameof(ActualTenantId)]);
    private set => Data[nameof(ActualTenantId)] = value?.Value;
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="TenantMismatchException"/> class.
  /// </summary>
  /// <param name="expected">The expected tenant identifier.</param>
  /// <param name="actual">The actual tenant identifier.</param>
  public TenantMismatchException(TenantId? expected, TenantId? actual)
    : base(BuildMessage(expected, actual))
  {
    ExpectedTenantId = expected;
    ActualTenantId = actual;
  }

  private static string BuildMessage(TenantId? expected, TenantId? actual) => new ErrorMessageBuilder(ErrorMessage)
    .AddData(nameof(ExpectedTenantId), expected?.Value, "<null>")
    .AddData(nameof(ActualTenantId), actual?.Value, "<null>")
    .Build();
}
