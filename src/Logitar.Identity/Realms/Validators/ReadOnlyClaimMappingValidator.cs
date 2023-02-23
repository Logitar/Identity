using FluentValidation;
using System.Security.Claims;

namespace Logitar.Identity.Realms.Validators;

/// <summary>
/// The validator used to validate instances of the <see cref="ReadOnlyClaimMapping"/> record.
/// </summary>
internal class ReadOnlyClaimMappingValidator : AbstractValidator<ReadOnlyClaimMapping>
{
  /// <summary>
  /// The list of allowed claim value types.
  /// </summary>
  private static readonly HashSet<string> _claimValueTypes = new(new[]
  {
    ClaimValueTypes.Base64Binary,
    ClaimValueTypes.Base64Octet,
    ClaimValueTypes.Boolean,
    ClaimValueTypes.Date,
    ClaimValueTypes.DateTime,
    ClaimValueTypes.DaytimeDuration,
    ClaimValueTypes.DnsName,
    ClaimValueTypes.Double,
    ClaimValueTypes.DsaKeyValue,
    ClaimValueTypes.Email,
    ClaimValueTypes.Fqbn,
    ClaimValueTypes.HexBinary,
    ClaimValueTypes.Integer,
    ClaimValueTypes.Integer32,
    ClaimValueTypes.Integer64,
    ClaimValueTypes.KeyInfo,
    ClaimValueTypes.Rfc822Name,
    ClaimValueTypes.Rsa,
    ClaimValueTypes.RsaKeyValue,
    ClaimValueTypes.Sid,
    ClaimValueTypes.String,
    ClaimValueTypes.Time,
    ClaimValueTypes.UInteger32,
    ClaimValueTypes.UInteger64,
    ClaimValueTypes.UpnName,
    ClaimValueTypes.X500Name,
    ClaimValueTypes.YearMonthDuration
  });

  /// <summary>
  /// Initializes a new intance of the <see cref="ReadOnlyClaimMappingValidator"/> class.
  /// </summary>
  public ReadOnlyClaimMappingValidator()
  {
    RuleFor(x => x.ClaimType).NotEmpty()
      .MaximumLength(byte.MaxValue);
    RuleFor(x => x.ClaimValueType).NullOrNotEmpty()
      .Must(x => x == null || _claimValueTypes.Contains(x))
      .WithErrorCode("ClaimValueTypeValidator")
      .WithMessage("'{PropertyName}' must be a valid claim value type defined in the 'System.Security.Claims.ClaimValueTypes' class.");
  }
}
