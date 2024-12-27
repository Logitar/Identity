# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

Nothing yet.

## [3.0.4] - 2024-12-27

### Fixed

- Session ID on user `SignIn` method.

## [3.0.3] - 2024-12-27

### Fixed

- Dependency Injection.

## [3.0.2] - 2024-12-27

### Added

- Some integration tests.

### Fixed

- EventBus Mediator is now public.
- API key expiration.

## [3.0.1] - 2024-12-23

### Added

- Value object unit tests.

### Fixed

- EventBus is now a public class and can be overriden.
- Locale constructor.
- Url constructor.

## [3.0.0] - 2024-12-23

### Changed

- Rewrote the framework to include changes made to EventSourcing.

## [2.0.0] - 2024-06-06

### Changed

- Contracts project now targets .NET Standard 2.1.
- Refactored domain events and aggregates.
- Upgraded NuGet packages.

## [1.0.0] - 2024-03-25

### Added

- Role, API key, User, Session, One-Time Password (OTP) domain events and aggregates.
- Password (PBKDF2) and JSON Web Token management.
- Relational storage (PostgreSQL and Microsoft SQL Server) for Identity entities.
- Unit and Integration tests.

[unreleased]: https://github.com/Logitar/Identity/compare/v3.0.4...HEAD
[3.0.4]: https://github.com/Logitar/Identity/compare/v3.0.3...v3.0.4
[3.0.3]: https://github.com/Logitar/Identity/compare/v3.0.2...v3.0.3
[3.0.2]: https://github.com/Logitar/Identity/compare/v3.0.1...v3.0.2
[3.0.1]: https://github.com/Logitar/Identity/compare/v3.0.0...v3.0.1
[3.0.0]: https://github.com/Logitar/Identity/compare/v2.0.0...v3.0.0
[2.0.0]: https://github.com/Logitar/Identity/compare/v1.0.0...v2.0.0
[1.0.0]: https://github.com/Logitar/Identity/releases/tag/v1.0.0
