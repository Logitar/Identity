namespace Logitar.Identity.Domain.Users;

[Trait(Traits.Category, Categories.Unit)]
public class PhoneExtensionsTests
{
  [Fact(DisplayName = "IsValid: it should return false when the phone is not valid.")]
  public void IsValid_it_should_return_false_when_the_phone_is_not_valid()
  {
    PhoneMock phone = new("ABCDEFGHIJ", "FR", extension: null);
    Assert.False(phone.IsValid());
  }

  [Fact(DisplayName = "IsValid: it should return true when the phone is valid.")]
  public void IsValid_it_should_return_true_when_the_phone_is_valid()
  {
    PhoneMock phone = new();
    Assert.True(phone.IsValid());
  }
}
