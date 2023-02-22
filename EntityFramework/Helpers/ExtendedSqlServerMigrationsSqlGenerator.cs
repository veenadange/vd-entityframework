using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Update;
using System.Diagnostics;

namespace EntityFrameworkRls.Helpers
{
    //Custom SqlServer migrations generator with overriden required operations (create/drop table)
    internal class ExtendedSqlServerMigrationsSqlGenerator : SqlServerMigrationsSqlGenerator
    {
        public ExtendedSqlServerMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IRelationalAnnotationProvider migrationsAnnotations)
            : base(dependencies, migrationsAnnotations) {
        }

        protected override void Generate(
            CreateTableOperation createTableOperation, IModel? model, MigrationCommandListBuilder builder, bool terminate)
        {
            //Debugger.Launch();
            //base class method called for CreateTableOperation script generation - before adding filter/block predicates for the table in security policy
            base.Generate(createTableOperation, model, builder, true);

            //if table has RLS annotation (set in dbcontext), add filter/block predicate for it in security policy
            //bool enableRlsPolicy = model?.GetEntityTypes()?.FirstOrDefault(x => (x.GetTableName()??string.Empty).Equals(createTableOperation.Name))?.FindAnnotation(new RlsPolicy().Name) != null;

            //if table has EnableRls attribute (set in dbcontext), add filter/block predicate for it in security policy
            var rlsTables = MigrationHelper.GetTableNamesForRls();
            bool enableRlsPolicy = rlsTables != null ? rlsTables.Contains(createTableOperation.Name) : false;
            if (enableRlsPolicy)
            {
                builder.AppendLine(
                $"EXEC('ALTER SECURITY POLICY [Rls].[tenantAccessPolicy]" + Environment.NewLine +
                $"ADD FILTER PREDICATE [Rls].[fn_tenantAccessPredicate](TenantId) ON {Dependencies.SqlGenerationHelper.DelimitIdentifier(createTableOperation.Name, createTableOperation.Schema)}," + Environment.NewLine +
                $"ADD BLOCK PREDICATE [Rls].[fn_tenantAccessPredicate](TenantId) ON {Dependencies.SqlGenerationHelper.DelimitIdentifier(createTableOperation.Name, createTableOperation.Schema)}')" + Environment.NewLine);
                
                builder.EndCommand();
            }
        }

        //gets executed in case of droptable e.g. rollback migration for createtable/table entity removal from dbcontext
        protected override void Generate(
            DropTableOperation dropTableOperation, IModel? model, MigrationCommandListBuilder builder, bool terminate)
        {
            //Debugger.Launch();

            //if table has RLS data annotation (set in dbcontext), drop filter/block predicate for it from security policy
            //bool enableRlsPolicy = model?.GetEntityTypes()?.FirstOrDefault(x => (x.GetTableName() ?? string.Empty).Equals(dropTableOperation.Name))?.FindAnnotation(new RlsPolicy().Name) != null;

            //if table has EnableRls attribute (set in dbcontext), drop filter/block predicate for it from security policy
            var rlsTables = MigrationHelper.GetTableNamesForRls();
            bool enableRlsPolicy = rlsTables != null ? rlsTables.Contains(dropTableOperation.Name) : false;

            if (enableRlsPolicy)
            {
                builder.AppendLine(
                $"EXEC('ALTER SECURITY POLICY [Rls].[tenantAccessPolicy]" + Environment.NewLine +
                $"DROP FILTER PREDICATE ON {Dependencies.SqlGenerationHelper.DelimitIdentifier(dropTableOperation.Name, dropTableOperation.Schema)}," + Environment.NewLine +
                $"DROP BLOCK PREDICATE ON {Dependencies.SqlGenerationHelper.DelimitIdentifier(dropTableOperation.Name, dropTableOperation.Schema)}')" + Environment.NewLine);

                builder.EndCommand();
            }

            //base class method called for DropTableOperation script generation - after removing filter/block predicates for the table from security policy
            base.Generate(dropTableOperation, model, builder, true);
        }
    }
}
