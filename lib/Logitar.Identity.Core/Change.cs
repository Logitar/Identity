namespace Logitar.Identity.Core;

/// <summary>
/// Represents a change of value.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public record Change<T>
{
  /// <summary>
  /// Gets the changed value.
  /// </summary>
  public T? Value { get; }

  /// <summary>
  /// Initializes a new instance of the <see cref="Change{T}"/> class.
  /// </summary>
  /// <param name="value">The changed value.</param>
  public Change(T? value)
  {
    Value = value;
  }
}
