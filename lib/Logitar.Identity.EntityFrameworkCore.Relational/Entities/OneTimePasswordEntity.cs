using Logitar.Identity.Core;
using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Passwords.Events;
using System.Text.Json;

namespace Logitar.Identity.EntityFrameworkCore.Relational.Entities;

public sealed class OneTimePasswordEntity : AggregateEntity
{
  public int OneTimePasswordId { get; private set; }

  public string? TenantId { get; private set; }
  public string EntityId { get; private set; } = string.Empty;

  public string PasswordHash { get; private set; } = string.Empty;

  public DateTime? ExpiresOn { get; private set; }
  public int? MaximumAttempts { get; private set; }

  public int AttemptCount { get; private set; }
  public bool HasValidationSucceeded { get; private set; }

  public string? CustomAttributes { get; private set; }

  public OneTimePasswordEntity(OneTimePasswordCreated @event) : base(@event)
  {
    OneTimePasswordId oneTimePasswordId = new(@event.StreamId);
    TenantId = oneTimePasswordId.TenantId?.Value;
    EntityId = oneTimePasswordId.EntityId.Value;

    PasswordHash = @event.Password.Encode();

    ExpiresOn = @event.ExpiresOn?.ToUniversalTime();
    MaximumAttempts = @event.MaximumAttempts;
  }

  private OneTimePasswordEntity() : base()
  {
  }

  public void Fail(OneTimePasswordValidationFailed @event)
  {
    Update(@event);

    AttemptCount++;
  }

  public void Succeed(OneTimePasswordValidationSucceeded @event)
  {
    Update(@event);

    AttemptCount++;
    HasValidationSucceeded = true;
  }

  public void Update(OneTimePasswordUpdated @event)
  {
    base.Update(@event);

    Dictionary<string, string> customAttributes = GetCustomAttributes();
    foreach (KeyValuePair<Identifier, string?> customAttribute in @event.CustomAttributes)
    {
      if (customAttribute.Value == null)
      {
        customAttributes.Remove(customAttribute.Key.Value);
      }
      else
      {
        customAttributes[customAttribute.Key.Value] = customAttribute.Value;
      }
    }
    SetCustomAttributes(customAttributes);
  }

  public Dictionary<string, string> GetCustomAttributes()
  {
    return (CustomAttributes == null ? null : JsonSerializer.Deserialize<Dictionary<string, string>>(CustomAttributes)) ?? [];
  }
  private void SetCustomAttributes(Dictionary<string, string> customAttributes)
  {
    CustomAttributes = customAttributes.Count < 1 ? null : JsonSerializer.Serialize(customAttributes);
  }
}
