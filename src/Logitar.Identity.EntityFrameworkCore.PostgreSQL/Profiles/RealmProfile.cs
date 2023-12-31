﻿using AutoMapper;
using Logitar.Identity.EntityFrameworkCore.PostgreSQL.Entities;
using Logitar.Identity.Realms;
using System.Text.Json;

namespace Logitar.Identity.EntityFrameworkCore.PostgreSQL.Profiles;

/// <summary>
/// The profile used to configure mapping of realms.
/// </summary>
internal class RealmProfile : Profile
{
  /// <summary>
  /// Initializes a new instance of the <see cref="RealmProfile"/> class.
  /// </summary>
  public RealmProfile()
  {
    CreateMap<RealmEntity, Realm>()
      .IncludeBase<AggregateEntity, Aggregate>()
      .ForMember(x => x.Id, x => x.MapFrom(MappingHelper.ToGuid))
      .ForMember(x => x.UsernameSettings, x => x.MapFrom(GetUsernameSettings))
      .ForMember(x => x.PasswordSettings, x => x.MapFrom(GetPasswordSettings))
      .ForMember(x => x.ClaimMappings, x => x.MapFrom(GetClaimMappings))
      .ForMember(x => x.CustomAttributes, x => x.MapFrom(MappingHelper.GetCustomAttributes))
      .ForMember(x => x.GoogleOAuth2Configuration, x => x.MapFrom(GetGoogleOAuth2Configuration));
    CreateMap<ReadOnlyGoogleOAuth2Configuration, GoogleOAuth2Configuration>();
  }

  /// <summary>
  /// Resolves the user claim mappings from the specified realm entity.
  /// </summary>
  /// <param name="entity">The realm entity.</param>
  /// <param name="realm">The realm output model.</param>
  /// <returns>The user claim mappings.</returns>
  /// <exception cref="InvalidOperationException">The user claim mappings could not be deserialized.</exception>
  private static IEnumerable<ClaimMapping> GetClaimMappings(RealmEntity entity, Realm realm)
  {
    if (entity.ClaimMappings == null)
    {
      return Enumerable.Empty<ClaimMapping>();
    }

    Dictionary<string, ReadOnlyClaimMapping> claimMappings = JsonSerializer.Deserialize<Dictionary<string, ReadOnlyClaimMapping>>(entity.ClaimMappings)
      ?? throw new InvalidOperationException($"The user claim mappings could not be deserialized on realm 'AggregateId={entity.AggregateId}'.");

    return claimMappings.Select(claimMapping => new ClaimMapping
    {
      Key = claimMapping.Key,
      ClaimType = claimMapping.Value.ClaimType,
      ClaimValueType = claimMapping.Value.ClaimValueType
    });
  }

  /// <summary>
  /// Resolves the Google OAuth 2.0 configuration from the specified realm entity.
  /// </summary>
  /// <param name="entity">The realm entity.</param>
  /// <param name="realm">The realm output model.</param>
  /// <param name="member">The source member.</param>
  /// <param name="context">The mapping context.</param>
  /// <returns>The Google OAuth 2.0 configuration.</returns>
  /// <exception cref="InvalidOperationException">The external provider configuration could not be deserialized.</exception>
  private static GoogleOAuth2Configuration? GetGoogleOAuth2Configuration(RealmEntity entity, Realm realm, GoogleOAuth2Configuration? member, ResolutionContext context)
  {
    if (entity.GoogleOAuth2Configuration == null)
    {
      return null;
    }

    ReadOnlyGoogleOAuth2Configuration googleOAuth2Configuration = JsonSerializer.Deserialize<ReadOnlyGoogleOAuth2Configuration>(entity.GoogleOAuth2Configuration)
      ?? throw new InvalidOperationException($"The Google OAuth 2.0 provider authentication configuration could not be deserialized on realm 'Id={entity.RealmId}'.");

    return context.Mapper.Map<GoogleOAuth2Configuration>(googleOAuth2Configuration);
  }

  /// <summary>
  /// Resolves the password settings from the specified realm entity.
  /// </summary>
  /// <param name="entity">The realm entity.</param>
  /// <param name="realm">The realm output model.</param>
  /// <returns>The password settings.</returns>
  /// <exception cref="InvalidOperationException">The password settings could not be deserialized.</exception>
  private static PasswordSettings GetPasswordSettings(RealmEntity entity, Realm realm)
  {
    return JsonSerializer.Deserialize<PasswordSettings>(entity.PasswordSettings)
      ?? throw new InvalidOperationException($"The password settings could not be deserialized on realm 'Id={entity.RealmId}'.");
  }

  /// <summary>
  /// Resolves the username settings from the specified realm entity.
  /// </summary>
  /// <param name="entity">The realm entity.</param>
  /// <param name="realm">The realm output model.</param>
  /// <returns>The username settings.</returns>
  /// <exception cref="InvalidOperationException">The username settings could not be deserialized.</exception>
  private static UsernameSettings GetUsernameSettings(RealmEntity entity, Realm realm)
  {
    return JsonSerializer.Deserialize<UsernameSettings>(entity.UsernameSettings)
      ?? throw new InvalidOperationException($"The username settings could not be deserialized on realm 'Id={entity.RealmId}'.");
  }
}
