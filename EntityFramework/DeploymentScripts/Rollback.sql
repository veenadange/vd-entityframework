BEGIN TRANSACTION;
GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222091719_Initial')
BEGIN
    EXEC('ALTER SECURITY POLICY [Rls].[tenantAccessPolicy]
    DROP FILTER PREDICATE ON [Pt].[Clients],
    DROP BLOCK PREDICATE ON [Pt].[Clients]')

END;
GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222091719_Initial')
BEGIN
    DROP TABLE [Pt].[Clients];
END;
GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222091719_Initial')
BEGIN
    DROP TABLE [Pt].[TaxReturns];
END;
GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222091719_Initial')
BEGIN
    DELETE FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230222091719_Initial';
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222091602_CreateRlsEntities')
BEGIN
    IF OBJECT_ID(N'[Rls].[tenantAccessPolicy]') IS NOT NULL
    BEGIN

    DROP SECURITY POLICY [Rls].[tenantAccessPolicy]

    END
END;
GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222091602_CreateRlsEntities')
BEGIN
    IF OBJECT_ID(N'[Rls].[fn_tenantAccessPredicate]') IS NOT NULL
    BEGIN

    EXEC('DROP FUNCTION [Rls].[fn_tenantAccessPredicate]')

    END
END;
GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222091602_CreateRlsEntities')
BEGIN
    IF SCHEMA_ID(N'Rls') IS NOT NULL EXEC(N'DROP SCHEMA [Rls];');
END;
GO

IF EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222091602_CreateRlsEntities')
BEGIN
    DELETE FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20230222091602_CreateRlsEntities';
END;
GO

COMMIT;
GO

