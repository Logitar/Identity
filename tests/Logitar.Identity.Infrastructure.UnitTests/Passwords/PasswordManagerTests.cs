using Logitar.Identity.Domain.Passwords;
using Logitar.Identity.Domain.Settings;
using Logitar.Identity.Infrastructure.Passwords.Pbkdf2;
using Moq;

namespace Logitar.Identity.Infrastructure.Passwords;

[Trait(Traits.Category, Categories.Unit)]
public class PasswordManagerTests
{
  private const string PasswordString = "P@s$W0rD";

  private readonly Pbkdf2Settings _pbkdf2Settings = new();
  private readonly UserSettings _userSettings = new();

  private readonly Mock<IUserSettingsResolver> _settingsResolver = new();
  private readonly IPasswordStrategy[] _strategies;
  private readonly PasswordManagerMock _passwordManager;

  public PasswordManagerTests()
  {
    _strategies = [new Base64Strategy(), new Pbkdf2Strategy(_pbkdf2Settings)];

    _settingsResolver.Setup(x => x.Resolve()).Returns(_userSettings);

    _passwordManager = new(_settingsResolver.Object, _strategies);
  }

  [Fact(DisplayName = "Create: it should create the correct password.")]
  public void Create_it_should_create_the_correct_password()
  {
    Password password = _passwordManager.Create(PasswordString);
    Assert.True(password is Pbkdf2Password);
    Assert.True(password.IsMatch(PasswordString));
  }

  [Fact(DisplayName = "Create: it should create the correct weak password without validation.")]
  public void Create_it_should_create_the_correct_weak_password_without_validation()
  {
    string passwordString = "AAaa!!11";
    Password password = _passwordManager.Create(passwordString, validate: false);
    Assert.True(password is Pbkdf2Password);
    Assert.True(password.IsMatch(passwordString));
  }

  [Fact(DisplayName = "Create: it should throw PasswordStrategyNotSupportedException when the strategy could not be found.")]
  public void Create_it_should_throw_PasswordStrategyNotSupportedException_when_the_strategy_could_not_be_found()
  {
    _userSettings.Password = new PasswordSettings
    {
      HashingStrategy = "PLAIN_TEXT"
    };
    Assert.Throws<PasswordStrategyNotSupportedException>(() => _passwordManager.Create(PasswordString));
  }

  [Fact(DisplayName = "Create: it should throw ValidationException when the password is too weak.")]
  public void Create_it_should_throw_ValidationException_when_the_password_is_too_weak()
  {
    FluentValidation.ValidationException exception;

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _passwordManager.Create(password: "AAaa!!11"));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "PasswordRequiresUniqueChars");

    exception = Assert.Throws<FluentValidation.ValidationException>(() => _passwordManager.Create(password: "AAaa!!11", validate: true));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "PasswordRequiresUniqueChars");
  }

  [Fact(DisplayName = "ctor: it should construct the correct password manager.")]
  public void ctor_it_should_construct_the_correct_password_manager()
  {
    Assert.Same(_settingsResolver.Object, _passwordManager.SettingsResolver);

    Assert.Equal(_strategies.Length, _passwordManager.Strategies.Count);
    foreach (IPasswordStrategy strategy in _strategies)
    {
      Assert.True(_passwordManager.Strategies.ContainsKey(strategy.Key));
      Assert.Same(strategy, _passwordManager.Strategies[strategy.Key]);
    }
  }

  [Fact(DisplayName = "Decode: it should decode the correct password.")]
  public void Decode_it_should_decode_the_correct_password()
  {
    Base64Password original = new(PasswordString);
    Password password = _passwordManager.Decode(original.Encode());
    Assert.True(password is Base64Password);
    Assert.True(password.IsMatch(PasswordString));
  }

  [Fact(DisplayName = "Decode: it should throw PasswordStrategyNotSupportedException when the strategy could not be found.")]
  public void Decode_it_should_throw_PasswordStrategyNotSupportedException_when_the_strategy_could_not_be_found()
  {
    string password = $"PLAIN_TEXT:{PasswordString}";
    Assert.Throws<PasswordStrategyNotSupportedException>(() => _passwordManager.Decode(password));
  }

  [Theory(DisplayName = "Generate: it should generate the correct password.")]
  [InlineData(3)]
  [InlineData(20)]
  public void Generate_it_should_generate_the_correct_password(int length)
  {
    Password password = _passwordManager.Generate(length, out byte[] passwordBytes);
    Assert.Equal(length, passwordBytes.Length);
    Assert.True(password.IsMatch(Convert.ToBase64String(passwordBytes)));
  }

  [Fact(DisplayName = "Generate: it should throw PasswordStrategyNotSupportedException when the strategy could not be found.")]
  public void Generate_it_should_throw_PasswordStrategyNotSupportedException_when_the_strategy_could_not_be_found()
  {
    _userSettings.Password = new PasswordSettings
    {
      HashingStrategy = "PLAIN_TEXT"
    };
    Assert.Throws<PasswordStrategyNotSupportedException>(() => _passwordManager.Generate(32, out _));
  }

  [Fact(DisplayName = "GetStrategy: it should return the correct strategy when found.")]
  public void GetStrategy_it_should_return_the_correct_strategy_when_found()
  {
    foreach (IPasswordStrategy strategy in _strategies)
    {
      Assert.Same(strategy, _passwordManager.GetStrategy(strategy.Key));
    }
  }

  [Fact(DisplayName = "GetStrategy: it should throw PasswordStrategyNotSupportedException when the strategy could not be found.")]
  public void GetStrategy_it_should_throw_PasswordStrategyNotSupportedException_when_the_strategy_could_not_be_found()
  {
    string key = "PLAIN_TEXT";
    var exception = Assert.Throws<PasswordStrategyNotSupportedException>(() => _passwordManager.GetStrategy(key));
    Assert.Equal(key, exception.Strategy);
  }
}
