using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace EntityFrameworkRls.Helpers
{
    //Custom migration sql generator with overriden required operations (create/drop table)
    internal class CustomSqlServerMigrationsSqlGenerator : SqlServerMigrationsSqlGenerator
    {
        public CustomSqlServerMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IRelationalAnnotationProvider migrationsAnnotations)
            : base(dependencies, migrationsAnnotations) {
        }

        protected override void Generate(
            CreateTableOperation createTableOperation, IModel? model, MigrationCommandListBuilder builder, bool terminate)
        {
            //base class method called for CreateTableOperation script generation - before applying policy to the table
            base.Generate(createTableOperation, model, builder, true);

            //if table has RLS data annotation Add filter/block predicate for it in security policy
            bool enableRlsPolicy = model?.GetEntityTypes()?.FirstOrDefault()?.FindAnnotation(new RlsPolicy().Name) != null;
            if (enableRlsPolicy)
            {
                //though AddSecurityPolicy() will get executed at each create table operation - no side effect - idempotent scripts
                //other way could be - a separate initial migration can also be created just for one time rls related entities - predicate func + empty security policy
                //in that case there is no need to add it to script for each create table operation
                builder.AddSecurityPolicy();

                builder.AppendLine(
                $"EXEC('ALTER SECURITY POLICY [Rls].[tenantAccessPolicy]" + Environment.NewLine +
                $"ADD FILTER PREDICATE [Rls].[fn_tenantAccessPredicate](TenantId) ON {Dependencies.SqlGenerationHelper.DelimitIdentifier(createTableOperation.Name, createTableOperation.Schema)}," + Environment.NewLine +
                $"ADD BLOCK PREDICATE [Rls].[fn_tenantAccessPredicate](TenantId) ON {Dependencies.SqlGenerationHelper.DelimitIdentifier(createTableOperation.Name, createTableOperation.Schema)}')" + Environment.NewLine);
                
                builder.EndCommand();
            }
        }

        //gets executed in case of droptable e.g. rollback migration for createtable/table removal from context
        protected override void Generate(
            DropTableOperation dropTableOperation, IModel? model, MigrationCommandListBuilder builder, bool terminate)
        {
            //if table has RLS data annotation drop filter/block predicate for it from security policy
            bool enableRlsPolicy = model?.GetEntityTypes()?.FirstOrDefault()?.FindAnnotation(new RlsPolicy().Name) != null;
            if (enableRlsPolicy)
            {
                builder.AppendLine(
                $"EXEC('ALTER SECURITY POLICY [Rls].[tenantAccessPolicy]" + Environment.NewLine +
                $"DROP FILTER PREDICATE ON {Dependencies.SqlGenerationHelper.DelimitIdentifier(dropTableOperation.Name, dropTableOperation.Schema)}," + Environment.NewLine +
                $"DROP BLOCK PREDICATE ON {Dependencies.SqlGenerationHelper.DelimitIdentifier(dropTableOperation.Name, dropTableOperation.Schema)}')" + Environment.NewLine);

                builder.EndCommand();
            }

            //base class method called for DropTableOperation script generation - after removing the table from policy in previous statements
            base.Generate(dropTableOperation, model, builder, true);
        }

        //Note: Below override for SqlServerCreateDatabaseOperation was tried but can not be used as it doesnot reflect commands in generated sql script
        //e.g. dotnet ef migrations script -i (used in deployment)
        //only useful when update-database cmd used

        //protected override void Generate(
        //    SqlServerCreateDatabaseOperation createDatabaseOperation, IModel? model, MigrationCommandListBuilder builder)
        //{
        //    Debugger.Launch();
        //    base.Generate(createDatabaseOperation, model, builder);

        //    builder.AppendLine($"USE {createDatabaseOperation.Name};");
        //    builder.EndCommand();
        //    builder.AppendLine($"CREATE SCHEMA Rls;");
        //    builder.EndCommand();

        //    builder.AppendLine(
        //    $"CREATE FUNCTION Rls.fn_tenantAccessPredicate(@TenantId uniqueidentifier)" +
        //    "RETURNS TABLE" + Environment.NewLine +
        //    "WITH SCHEMABINDING" + Environment.NewLine +
        //    "AS" + Environment.NewLine +
        //        "RETURN SELECT 1 AS fn_accessResult" + Environment.NewLine +
        //        "WHERE CAST(SESSION_CONTEXT(N'TenantId') AS uniqueidentifier) = @TenantId;" + Environment.NewLine
        //    );
        //    builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
        //    builder.EndCommand();

        //    builder.AppendLine(
        //    $"CREATE SECURITY POLICY Rls.tenantAccessPolicy" + Environment.NewLine);
        //    builder.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
        //    builder.EndCommand();
        //}
    }
}
