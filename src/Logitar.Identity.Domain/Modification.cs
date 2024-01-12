namespace Logitar.Identity.Domain;

/// <summary>
/// Represents the modification of a value.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public record Modification<T>
{
  /// <summary>
  /// Gets the modified value.
  /// </summary>
  public T? Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Modification{T}"/> class.
  /// </summary>
  /// <param name="value">The modified value.</param>
  public Modification(T? value)
  {
    Value = value;
  }
}
