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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230221062243_initial')
BEGIN
    IF SCHEMA_ID(N'Pt') IS NULL EXEC(N'CREATE SCHEMA [Pt];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230221062243_initial')
BEGIN
    CREATE TABLE [Pt].[Clients] (
        [Id] uniqueidentifier NOT NULL,
        [TenantId] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Address] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Clients] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230221062243_initial')
BEGIN
    IF SCHEMA_ID(N'Rls') IS NULL EXEC(N'CREATE SCHEMA [Rls];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230221062243_initial')
BEGIN
    IF OBJECT_ID(N'[Rls].[fn_tenantAccessPredicate]') IS NULL
    BEGIN

    EXEC('CREATE FUNCTION [Rls].[fn_tenantAccessPredicate](@TenantId uniqueidentifier)RETURNS TABLE
    WITH SCHEMABINDING
    AS
    RETURN SELECT 1 AS fn_accessResult
    WHERE CAST(SESSION_CONTEXT(N''TenantId'') AS uniqueidentifier) = @TenantId;')

    END
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230221062243_initial')
BEGIN
    IF OBJECT_ID(N'[Rls].[tenantAccessPolicy]') IS NULL
    BEGIN

    CREATE SECURITY POLICY [Rls].[tenantAccessPolicy]

    END
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230221062243_initial')
BEGIN
    EXEC('ALTER SECURITY POLICY [Rls].[tenantAccessPolicy]
    ADD FILTER PREDICATE [Rls].[fn_tenantAccessPredicate](TenantId) ON [Pt].[Clients],
    ADD BLOCK PREDICATE [Rls].[fn_tenantAccessPredicate](TenantId) ON [Pt].[Clients]')

END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230221062243_initial')
BEGIN
    CREATE TABLE [Pt].[TaxReturns] (
        [Id] uniqueidentifier NOT NULL,
        [TenantId] uniqueidentifier NOT NULL,
        [TaxYear] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_TaxReturns] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230221062243_initial')
BEGIN
    IF SCHEMA_ID(N'Rls') IS NULL EXEC(N'CREATE SCHEMA [Rls];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230221062243_initial')
BEGIN
    IF OBJECT_ID(N'[Rls].[fn_tenantAccessPredicate]') IS NULL
    BEGIN

    EXEC('CREATE FUNCTION [Rls].[fn_tenantAccessPredicate](@TenantId uniqueidentifier)RETURNS TABLE
    WITH SCHEMABINDING
    AS
    RETURN SELECT 1 AS fn_accessResult
    WHERE CAST(SESSION_CONTEXT(N''TenantId'') AS uniqueidentifier) = @TenantId;')

    END
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230221062243_initial')
BEGIN
    IF OBJECT_ID(N'[Rls].[tenantAccessPolicy]') IS NULL
    BEGIN

    CREATE SECURITY POLICY [Rls].[tenantAccessPolicy]

    END
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230221062243_initial')
BEGIN
    EXEC('ALTER SECURITY POLICY [Rls].[tenantAccessPolicy]
    ADD FILTER PREDICATE [Rls].[fn_tenantAccessPredicate](TenantId) ON [Pt].[TaxReturns],
    ADD BLOCK PREDICATE [Rls].[fn_tenantAccessPredicate](TenantId) ON [Pt].[TaxReturns]')

END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230221062243_initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230221062243_initial', N'6.0.14');
END;
GO

COMMIT;
GO

