namespace Logitar.Identity.Domain;

public record Modification<T>
{
  public T? Value { get; }

  public Modification(T? value)
  {
    Value = value;
  }
}
