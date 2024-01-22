using Logitar.Identity.Domain.Passwords.Events;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public class OneTimePasswordEntity : AggregateEntity
{
  public int OneTimePasswordId { get; private set; }

  public string? TenantId { get; private set; }

  public string PasswordHash { get; private set; } = string.Empty;

  public DateTime? ExpiresOn { get; private set; }
  public int? MaximumAttempts { get; private set; }

  public int AttemptCount { get; private set; }
  public bool HasValidationSucceeded { get; private set; }

  public Dictionary<string, string> CustomAttributes { get; private set; } = [];
  public string? CustomAttributesSerialized
  {
    get => CustomAttributes.Count == 0 ? null : JsonSerializer.Serialize(CustomAttributes);
    private set
    {
      if (value == null)
      {
        CustomAttributes.Clear();
      }
      else
      {
        CustomAttributes = JsonSerializer.Deserialize<Dictionary<string, string>>(value) ?? [];
      }
    }
  }

  public OneTimePasswordEntity(OneTimePasswordCreatedEvent @event) : base(@event)
  {
    TenantId = @event.TenantId?.Value;

    PasswordHash = @event.Password.Encode();

    ExpiresOn = @event.ExpiresOn;
    MaximumAttempts = @event.MaximumAttempts;
  }

  private OneTimePasswordEntity() : base()
  {
  }

  public void Fail(OneTimePasswordValidationFailedEvent @event)
  {
    Update(@event);

    AttemptCount++;
  }

  public void Succeed(OneTimePasswordValidationSucceededEvent @event)
  {
    Update(@event);

    HasValidationSucceeded = true;
  }

  public void Update(OneTimePasswordUpdatedEvent @event)
  {
    base.Update(@event);

    foreach (KeyValuePair<string, string?> customAttribute in @event.CustomAttributes)
    {
      if (customAttribute.Value == null)
      {
        CustomAttributes.Remove(customAttribute.Key);
      }
      else
      {
        CustomAttributes[customAttribute.Key] = customAttribute.Value;
      }
    }
  }
}
