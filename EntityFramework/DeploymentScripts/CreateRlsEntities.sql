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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222083702_CreateRlsEntities')
BEGIN
    IF SCHEMA_ID(N'Rls') IS NULL EXEC(N'CREATE SCHEMA [Rls];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222083702_CreateRlsEntities')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222083702_CreateRlsEntities')
BEGIN
    IF OBJECT_ID(N'[Rls].[tenantAccessPolicy]') IS NULL
    BEGIN

    CREATE SECURITY POLICY [Rls].[tenantAccessPolicy]

    END
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222083702_CreateRlsEntities')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230222083702_CreateRlsEntities', N'7.0.3');
END;
GO

COMMIT;
GO

