using Logitar.Identity.Core.Passwords;
using Logitar.Identity.Core.Settings;
using Logitar.Identity.Infrastructure.Passwords.Pbkdf2;
using Moq;

namespace Logitar.Identity.Infrastructure.Passwords;

[Trait(Traits.Category, Categories.Unit)]
public class PasswordManagerTests
{
  private const string StrongPassword = "P@s$W0rD";
  private const string WeakPassword = "AAaa!!11";

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
    Password password = _passwordManager.Create(WeakPassword);
    Assert.True(password is Pbkdf2Password);
    Assert.True(password.IsMatch(WeakPassword));
  }

  [Fact(DisplayName = "Create: it should throw PasswordStrategyNotSupportedException when the strategy could not be found.")]
  public void Create_it_should_throw_PasswordStrategyNotSupportedException_when_the_strategy_could_not_be_found()
  {
    _userSettings.Password = new PasswordSettings
    {
      HashingStrategy = "PLAIN_TEXT"
    };
    Assert.Throws<PasswordStrategyNotSupportedException>(() => _passwordManager.Create(WeakPassword));
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
    Base64Password original = new(WeakPassword);
    Password password = _passwordManager.Decode(original.Encode());
    Assert.True(password is Base64Password);
    Assert.True(password.IsMatch(WeakPassword));
  }

  [Fact(DisplayName = "Decode: it should throw PasswordStrategyNotSupportedException when the strategy could not be found.")]
  public void Decode_it_should_throw_PasswordStrategyNotSupportedException_when_the_strategy_could_not_be_found()
  {
    string password = $"PLAIN_TEXT:{WeakPassword}";
    Assert.Throws<PasswordStrategyNotSupportedException>(() => _passwordManager.Decode(password));
  }

  [Theory(DisplayName = "GenerateBase64: it should generate the correct password.")]
  [InlineData(12)]
  [InlineData(32)]
  public void GenerateBase64_it_should_generate_the_correct_password(int length)
  {
    Password password = _passwordManager.GenerateBase64(length, out string passwordString);
    Assert.Equal(length, Convert.FromBase64String(passwordString).Length);
    Assert.True(password is Pbkdf2Password);
    Assert.True(password.IsMatch(passwordString));
  }

  [Theory(DisplayName = "Generate: it should generate a password from the default character set.")]
  [InlineData(20)]
  public void Generate_it_should_generate_a_password_from_the_default_character_set(int length)
  {
    Password password = _passwordManager.Generate(length, out string passwordString);
    Assert.Equal(length, passwordString.Length);
    Assert.True(password is Pbkdf2Password);
    Assert.True(password.IsMatch(passwordString));
  }

  [Theory(DisplayName = "Generate: it should generate a password from the specified character set.")]
  [InlineData("ACDEFGHJKLMNPQRSTUVWXYZ2345679", 6)]
  public void Generate_it_should_generate_a_password_from_the_default_specified_set(string characters, int length)
  {
    Password password = _passwordManager.Generate(characters, length, out string passwordString);
    Assert.Equal(length, passwordString.Length);
    Assert.True(passwordString.All(characters.Contains));
    Assert.True(password is Pbkdf2Password);
    Assert.True(password.IsMatch(passwordString));
  }

  [Fact(DisplayName = "Generate: it should throw PasswordStrategyNotSupportedException when the strategy could not be found.")]
  public void Generate_it_should_throw_PasswordStrategyNotSupportedException_when_the_strategy_could_not_be_found()
  {
    _userSettings.Password = new PasswordSettings
    {
      HashingStrategy = "PLAIN_TEXT"
    };
    Assert.Throws<PasswordStrategyNotSupportedException>(() => _passwordManager.Generate(length: 10, out _));

    _userSettings.Password = new PasswordSettings
    {
      HashingStrategy = "PLAIN_TEXT"
    };
    Assert.Throws<PasswordStrategyNotSupportedException>(() => _passwordManager.Generate(characters: "ACDEFGHJKLMNPQRSTUVWXYZ2345679", length: 10, out _));
  }

  [Fact(DisplayName = "GenerateBase64: it should throw PasswordStrategyNotSupportedException when the strategy could not be found.")]
  public void GenerateBase64_it_should_throw_PasswordStrategyNotSupportedException_when_the_strategy_could_not_be_found()
  {
    _userSettings.Password = new PasswordSettings
    {
      HashingStrategy = "PLAIN_TEXT"
    };
    Assert.Throws<PasswordStrategyNotSupportedException>(() => _passwordManager.GenerateBase64(length: 10, out _));
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

  [Fact(DisplayName = "Validate: it should succeed when the password is strong.")]
  public void Validate_it_should_succeed_when_the_password_is_strong()
  {
    _passwordManager.Validate(StrongPassword);
  }

  [Fact(DisplayName = "Validate: it should throw ValidationException when the password is too weak.")]
  public void Validate_it_should_throw_ValidationException_when_the_password_is_too_weak()
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => _passwordManager.Validate(WeakPassword));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "PasswordRequiresUniqueChars");
  }

  [Fact(DisplayName = "ValidateAndCreate: it should create the correct password.")]
  public void ValidateAndCreate_it_should_create_the_correct_password()
  {
    Password password = _passwordManager.ValidateAndCreate(StrongPassword);
    Assert.True(password is Pbkdf2Password);
    Assert.True(password.IsMatch(StrongPassword));
  }

  [Fact(DisplayName = "ValidateAndCreate: it should throw PasswordStrategyNotSupportedException when the strategy could not be found.")]
  public void ValidateAndCreate_it_should_throw_PasswordStrategyNotSupportedException_when_the_strategy_could_not_be_found()
  {
    _userSettings.Password = new PasswordSettings
    {
      HashingStrategy = "PLAIN_TEXT"
    };
    Assert.Throws<PasswordStrategyNotSupportedException>(() => _passwordManager.ValidateAndCreate(StrongPassword));
  }

  [Fact(DisplayName = "ValidateAndCreate: it should throw ValidationException when the password is too weak.")]
  public void ValidateAndCreate_it_should_throw_ValidationException_when_the_password_is_too_weak()
  {
    var exception = Assert.Throws<FluentValidation.ValidationException>(() => _passwordManager.ValidateAndCreate(WeakPassword));
    Assert.Contains(exception.Errors, e => e.ErrorCode == "PasswordRequiresUniqueChars");
  }
}
