using FluentValidation.Results;
using Logitar.Identity.Domain.Settings;

namespace Logitar.Identity.Domain.Passwords;

[Trait(Traits.Category, Categories.Unit)]
public class PasswordValidatorTests
{
  private readonly PasswordSettings _settings = new();
  private readonly PasswordValidator _validator;

  public PasswordValidatorTests()
  {
    _validator = new(_settings);
  }

  [Fact(DisplayName = "Validation should fail when the password does not contain a digit character.")]
  public void Validation_should_fail_when_the_password_does_not_contain_a_digit_character()
  {
    ValidationResult result = _validator.Validate("AAaa!!!!");
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "PasswordRequiresDigit");
  }

  [Fact(DisplayName = "Validation should fail when the password does not contain a lowercase character.")]
  public void Validation_should_fail_when_the_password_does_not_contain_a_lowercase_character()
  {
    ValidationResult result = _validator.Validate("AAAA!!11");
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "PasswordRequiresLower");
  }

  [Fact(DisplayName = "Validation should fail when the password does not contain a non-alphanumeric character.")]
  public void Validation_should_fail_when_the_password_does_not_contain_a_non_alphanumeric_character()
  {
    ValidationResult result = _validator.Validate("AAaa1111");
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "PasswordRequiresNonAlphanumeric");
  }

  [Fact(DisplayName = "Validation should fail when the password does not contain an uppercase character.")]
  public void Validation_should_fail_when_the_password_does_not_contain_an_uppercase_character()
  {
    ValidationResult result = _validator.Validate("aaaa!!11");
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "PasswordRequiresUpper");
  }

  [Fact(DisplayName = "Validation should fail when the password does not contain enough unique characters.")]
  public void Validation_should_fail_when_the_password_does_not_contain_enough_unique_characters()
  {
    ValidationResult result = _validator.Validate("AAaa!!11");
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "PasswordRequiresUniqueChars");
  }

  [Fact(DisplayName = "Validation should fail when the password is too short.")]
  public void Validation_should_fail_when_the_password_is_too_short()
  {
    ValidationResult result = _validator.Validate("Aa!1");
    Assert.False(result.IsValid);
    Assert.Contains(result.Errors, e => e.ErrorCode == "PasswordTooShort");
  }

  [Fact(DisplayName = "Validation should succeed when criterias are met.")]
  public void Validation_should_succeed_when_criterias_are_met()
  {
    ValidationResult result = _validator.Validate("Test123!");
    Assert.True(result.IsValid);
    Assert.Empty(result.Errors);
  }
}
