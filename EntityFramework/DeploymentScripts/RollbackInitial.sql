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

