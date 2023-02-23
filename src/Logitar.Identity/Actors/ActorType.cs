namespace Logitar.Identity.Actors;

/**************************************************************************************************/
/* WARNING - DO NOT CHANGE THE NAMES AND VALUES OF EXISTING PROVIDERS SINCE THEY WILL BE          */
/* PERSISTED IN THE DATA STORES.                                                                  */
/**************************************************************************************************/

/// <summary>
/// Represents the possible types of actors.
/// </summary>
public enum ActorType
{
  /// <summary>
  /// The type of the default system actor.
  /// </summary>
  System = 0,

  /// <summary>
  /// The type used for API key actors.
  /// </summary>
  ApiKey,

  /// <summary>
  /// The type used for user actors.
  /// </summary>
  User
}
