using Logitar.EventSourcing;
using Logitar.Identity.Domain.Shared;

public class RoleAggregate : AggregateRoot
{
  /// <summary>
  /// Gets the tenant identifier of the role.
  /// </summary>
  public TenantId? TenantId { get; private set; }

  /// <summary>
  /// Initializes a new instance of the <see cref="RoleAggregate"/> class.
  /// DO NOT use this constructor to create a new role. It is only intended to be used by the event sourcing.
  /// </summary>
  public RoleAggregate() : base()
  {
  }
}
