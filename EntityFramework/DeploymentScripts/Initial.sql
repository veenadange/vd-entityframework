BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222084145_Initial')
BEGIN
    IF SCHEMA_ID(N'Pt') IS NULL EXEC(N'CREATE SCHEMA [Pt];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222084145_Initial')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222084145_Initial')
BEGIN
    EXEC('ALTER SECURITY POLICY [Rls].[tenantAccessPolicy]
    ADD FILTER PREDICATE [Rls].[fn_tenantAccessPredicate](TenantId) ON [Pt].[Clients],
    ADD BLOCK PREDICATE [Rls].[fn_tenantAccessPredicate](TenantId) ON [Pt].[Clients]')

END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222084145_Initial')
BEGIN
    CREATE TABLE [Pt].[TaxReturns] (
        [Id] uniqueidentifier NOT NULL,
        [TenantId] uniqueidentifier NOT NULL,
        [TaxYear] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_TaxReturns] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20230222084145_Initial')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20230222084145_Initial', N'7.0.3');
END;
GO

COMMIT;
GO

