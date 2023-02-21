using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EntityFrameworkRls.Helpers
{
    public static class MigrationHelper
    {
        //adds custom rls annotation for table
        public static TableBuilder EnableRLS(this TableBuilder tableBuilder)
        {
            var policy = new RlsPolicy();
            tableBuilder.Metadata.SetAnnotation(policy.Name, policy.Value);
            return tableBuilder;
        }

        //builds idempotent scripts for creation of predicate function and security policy for rls
        public static void AddSecurityPolicy(this MigrationCommandListBuilder builder)
        {
            builder.AppendLine($"IF SCHEMA_ID(N'Rls') IS NULL EXEC(N'CREATE SCHEMA [Rls];');");
            builder.EndCommand();

            builder.AppendLine($"IF OBJECT_ID(N'[Rls].[fn_tenantAccessPredicate]') IS NULL");
            builder.AppendLine($"BEGIN{Environment.NewLine}");
            builder.AppendLine(
            $"EXEC('CREATE FUNCTION [Rls].[fn_tenantAccessPredicate](@TenantId uniqueidentifier)" +
            "RETURNS TABLE" + Environment.NewLine +
            "WITH SCHEMABINDING" + Environment.NewLine +
            "AS" + Environment.NewLine +
                "RETURN SELECT 1 AS fn_accessResult" + Environment.NewLine +
                "WHERE CAST(SESSION_CONTEXT(N''TenantId'') AS uniqueidentifier) = @TenantId;')" + Environment.NewLine
            );
            builder.AppendLine($"END");
            builder.EndCommand();

            builder.AppendLine($"IF OBJECT_ID(N'[Rls].[tenantAccessPolicy]') IS NULL");
            builder.AppendLine($"BEGIN{Environment.NewLine}");
            builder.AppendLine(
            $"CREATE SECURITY POLICY [Rls].[tenantAccessPolicy]" + Environment.NewLine);
            builder.AppendLine($"END");
            builder.EndCommand();
        }
    }
}
