IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Logs] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
    [LogLevel] nvarchar(max) NULL,
    [ThreadId] int NOT NULL,
    [EventId] int NOT NULL,
    [EventName] nvarchar(max) NULL,
    [Message] nvarchar(max) NULL,
    [Values] nvarchar(max) NULL,
    [Created] datetime2 NOT NULL,
    CONSTRAINT [PK_Logs] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [ProjectVersions] (
    [Code] nvarchar(450) NOT NULL,
    [Year] int NOT NULL,
    CONSTRAINT [PK_ProjectVersions] PRIMARY KEY ([Code])
);
GO

CREATE TABLE [Roles] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
    [UpdatePIC] nvarchar(max) NULL,
    [UpdatedDate] datetime2 NOT NULL DEFAULT (getdate()),
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Users] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
    [Code] nvarchar(max) NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [Password] nvarchar(max) NULL,
    [Email] nvarchar(256) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [IdentityNumber] nvarchar(255) NULL,
    [UpdatedPIC] nvarchar(max) NULL,
    [UpdatedDate] datetime2 NOT NULL DEFAULT (getdate()),
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [RoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_RoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_RoleClaims_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [PaperNews] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
    [Title] nvarchar(max) NOT NULL,
    [Abstract] nvarchar(max) NOT NULL,
    [ThumbImage] nvarchar(500) NULL,
    [ViewCount] bigint NOT NULL,
    [Content] nvarchar(max) NULL,
    [Status] int NOT NULL,
    [Slug] nvarchar(500) NULL,
    [UserId] uniqueidentifier NULL,
    [UpdatePIC] nvarchar(max) NULL,
    [CreatePIC] nvarchar(max) NULL,
    [UpdatedDate] datetime2 NOT NULL DEFAULT (getdate()),
    [CreatedDate] datetime2 NOT NULL DEFAULT (getdate()),
    [VersionCode] nvarchar(450) NULL,
    CONSTRAINT [PK_PaperNews] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PaperNews_ProjectVersions_VersionCode] FOREIGN KEY ([VersionCode]) REFERENCES [ProjectVersions] ([Code]),
    CONSTRAINT [FK_PaperNews_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);
GO

CREATE TABLE [Papers] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
    [ManuscriptTitle] nvarchar(max) NOT NULL,
    [Abstract] nvarchar(max) NOT NULL,
    [Submission] nvarchar(max) NOT NULL,
    [File] nvarchar(max) NOT NULL,
    [Status] int NOT NULL,
    [Keywords] nvarchar(max) NOT NULL,
    [UserId] uniqueidentifier NULL,
    [UpdatePIC] nvarchar(max) NULL,
    [CreatePIC] nvarchar(max) NULL,
    [UpdatedDate] datetime2 NOT NULL DEFAULT (getdate()),
    [CreatedDate] datetime2 NOT NULL DEFAULT (getdate()),
    [VersionCode] nvarchar(450) NULL,
    CONSTRAINT [PK_Papers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Papers_ProjectVersions_VersionCode] FOREIGN KEY ([VersionCode]) REFERENCES [ProjectVersions] ([Code]),
    CONSTRAINT [FK_Papers_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);
GO

CREATE TABLE [UserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_UserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_UserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_UserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserRoles] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [UserTokens] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_UserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_UserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [Authors] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
    [FirstName] nvarchar(max) NOT NULL,
    [MiddleName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Country] nvarchar(max) NOT NULL,
    [Affiliation] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [AuthorNum] int NOT NULL,
    [AuthorRole] int NULL,
    [Phone] nvarchar(max) NOT NULL,
    [PaperId] uniqueidentifier NULL,
    [UpdatePIC] nvarchar(max) NULL,
    [CreatePIC] nvarchar(max) NULL,
    [UpdatedDate] datetime2 NOT NULL DEFAULT (getdate()),
    [CreatedDate] datetime2 NOT NULL DEFAULT (getdate()),
    CONSTRAINT [PK_Authors] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Authors_Papers_PaperId] FOREIGN KEY ([PaperId]) REFERENCES [Papers] ([Id])
);
GO

CREATE TABLE [Registrations] (
    [Id] uniqueidentifier NOT NULL DEFAULT (NEWID()),
    [FirstName] nvarchar(max) NOT NULL,
    [MiddleName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Position] int NULL,
    [StudentId] nvarchar(max) NULL,
    [Affilication] nvarchar(max) NOT NULL,
    [CountryOrRegion] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [TelephoneNumber] nvarchar(max) NOT NULL,
    [PaperId] uniqueidentifier NULL,
    [PresentationType] int NULL,
    [RegistrationType] int NOT NULL,
    [AuthorRegular5VND] int NOT NULL,
    [AuthorRegular3USD] int NOT NULL,
    [AuthorRegular45VND] int NOT NULL,
    [AuthorRegular25USD] int NOT NULL,
    [Listener1VND] int NOT NULL,
    [Listener1USD] int NOT NULL,
    [BillTo] nvarchar(max) NOT NULL,
    [AnyRemark] nvarchar(max) NULL,
    [Diet] int NOT NULL,
    [DietComment] nvarchar(max) NULL,
    [Files] nvarchar(max) NULL,
    [Status] int NOT NULL,
    [UserId] uniqueidentifier NULL,
    [UpdatePIC] nvarchar(max) NULL,
    [CreatePIC] nvarchar(max) NULL,
    [UpdatedDate] datetime2 NOT NULL DEFAULT (getdate()),
    [CreatedDate] datetime2 NOT NULL DEFAULT (getdate()),
    [VersionCode] nvarchar(450) NULL,
    CONSTRAINT [PK_Registrations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Registrations_Papers_PaperId] FOREIGN KEY ([PaperId]) REFERENCES [Papers] ([Id]),
    CONSTRAINT [FK_Registrations_ProjectVersions_VersionCode] FOREIGN KEY ([VersionCode]) REFERENCES [ProjectVersions] ([Code]),
    CONSTRAINT [FK_Registrations_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Code', N'Year') AND [object_id] = OBJECT_ID(N'[ProjectVersions]'))
    SET IDENTITY_INSERT [ProjectVersions] ON;
INSERT INTO [ProjectVersions] ([Code], [Year])
VALUES (N'FCBEM24', 2024);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Code', N'Year') AND [object_id] = OBJECT_ID(N'[ProjectVersions]'))
    SET IDENTITY_INSERT [ProjectVersions] OFF;
GO

CREATE INDEX [IX_Authors_PaperId] ON [Authors] ([PaperId]);
GO

CREATE INDEX [IX_PaperNews_UserId] ON [PaperNews] ([UserId]);
GO

CREATE INDEX [IX_PaperNews_VersionCode] ON [PaperNews] ([VersionCode]);
GO

CREATE INDEX [IX_Papers_UserId] ON [Papers] ([UserId]);
GO

CREATE INDEX [IX_Papers_VersionCode] ON [Papers] ([VersionCode]);
GO

CREATE INDEX [IX_Registrations_PaperId] ON [Registrations] ([PaperId]);
GO

CREATE INDEX [IX_Registrations_UserId] ON [Registrations] ([UserId]);
GO

CREATE INDEX [IX_Registrations_VersionCode] ON [Registrations] ([VersionCode]);
GO

CREATE INDEX [IX_RoleClaims_RoleId] ON [RoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [Roles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_UserClaims_UserId] ON [UserClaims] ([UserId]);
GO

CREATE INDEX [IX_UserLogins_UserId] ON [UserLogins] ([UserId]);
GO

CREATE INDEX [IX_UserRoles_RoleId] ON [UserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [Users] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [Users] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240402121033_ver1.0.0.0', N'8.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Papers] ADD [PaperId] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240404081334_ver1.0.0.1', N'8.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Authors]') AND [c].[name] = N'AuthorRole');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Authors] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Authors] DROP COLUMN [AuthorRole];
GO

ALTER TABLE [Papers] ADD [AuthorRole] int NULL;
GO

ALTER TABLE [Authors] ADD [IsCorresponding] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240404115638_ver1.0.0.2', N'8.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Papers]') AND [c].[name] = N'AuthorRole');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Papers] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Papers] DROP COLUMN [AuthorRole];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240411125422_ver1.0.0.3', N'8.0.4');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Registrations] ADD [RegistrationId] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240423152455_ver1.0.0.4', N'8.0.4');
GO

COMMIT;
GO

