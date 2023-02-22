using System.Text;

namespace EntityFrameworkRls.Helpers
{
    public static class MigrationHelper
    {
        //builds idempotent script for creation of predicate function and security policy for rls
        public static StringBuilder BuildCreateSecurityPolicyScript()
        {
            StringBuilder builder = new();
            builder.AppendLine("GO");
            builder.AppendLine($"IF SCHEMA_ID(N'Rls') IS NULL EXEC(N'CREATE SCHEMA [Rls];');");
            builder.AppendLine("GO");

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
            builder.AppendLine("GO");

            builder.AppendLine($"IF OBJECT_ID(N'[Rls].[tenantAccessPolicy]') IS NULL");
            builder.AppendLine($"BEGIN{Environment.NewLine}");
            builder.AppendLine(
            $"CREATE SECURITY POLICY [Rls].[tenantAccessPolicy]" + Environment.NewLine);
            builder.AppendLine($"END");
            builder.AppendLine("GO");
            return builder;
        }

        //builds idempotent script for dropping predicate function and security policy for rls
        public static StringBuilder BuildDropSecurityPolicyScript()
        {
            StringBuilder builder = new();
            builder.AppendLine("GO");

            builder.AppendLine($"IF OBJECT_ID(N'[Rls].[tenantAccessPolicy]') IS NOT NULL");
            builder.AppendLine($"BEGIN{Environment.NewLine}");
            builder.AppendLine(
            $"DROP SECURITY POLICY [Rls].[tenantAccessPolicy]" + Environment.NewLine);
            builder.AppendLine($"END");
            builder.AppendLine("GO");

            builder.AppendLine($"IF OBJECT_ID(N'[Rls].[fn_tenantAccessPredicate]') IS NOT NULL");
            builder.AppendLine($"BEGIN{Environment.NewLine}");
            builder.AppendLine(
            $"EXEC('DROP FUNCTION [Rls].[fn_tenantAccessPredicate]')" + Environment.NewLine
            );
            builder.AppendLine($"END");
            builder.AppendLine("GO");

            builder.AppendLine($"IF SCHEMA_ID(N'Rls') IS NOT NULL EXEC(N'DROP SCHEMA [Rls];');");
            builder.AppendLine("GO");
            return builder;
        }

        //gets table names having custom attribute - [EnableRls] from dbcontext properties
        public static IEnumerable<string>? GetTableNamesForRls() {
            
            return typeof(PtDbContext).GetProperties().Where(x => Attribute.GetCustomAttribute(x, typeof(EnableRlsAttribute)) != null)?.Select(x => x.Name);
        }
    }
}
