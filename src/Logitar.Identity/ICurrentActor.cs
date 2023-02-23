using Logitar.EventSourcing;

namespace Logitar.Identity;

/// <summary>
/// Represents the acting context of the identity system.
/// </summary>
public interface ICurrentActor
{
  /// <summary>
  /// Gets the identifier of the current actor.
  /// </summary>
  AggregateId Id { get; }
}
