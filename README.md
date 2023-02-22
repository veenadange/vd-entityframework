# EF support RLS migration scripts
This repo implements an approach for adding/modifiying RLS (Row level security) related entities during EF migrations, by extending Sql service migration generator.

1. Implements custom SqlServer migrations generator - 'ExtendedSqlServerMigrationsSqlGenerator' extended from EF's 'SqlServerMigrationsSqlGenerator'.
2. 'Generate' method for 'CreateTableOperation'/'DropTableOperation' is overriden.
3. Replace migration generator service IMigrationsSqlGenerator with custom one in program.cs/startup.cs.
4. First empty migration needs to be added (without any code inside dbcontext), so that it will generate empty Up/Down methods. Added sql scripts in that for creating pre-requisite rls related entities (predicate function + empty security policy).
5. Created custom attribute 'EnableRlsAttribute'. Apply it to the required tables (dbsets) from dbcontext. This attribute is read inside overriden methods of 'ExtendedSqlServerMigrationsSqlGenerator'. It decides whether to add/drop filter/block predicate for a table, at the time of ef script generation/update-database operation.
6. This will automatically add/drop predicates for a table to security policy while creating/dropping it, provided that given custom attribute is set for a given table.

# Sample helper commands for script generation

dotnet ef migrations script 20230222083702_CreateRlsEntities 20230222084145_Initial -i -o DeploymentScripts/Initial.sql

dotnet ef migrations script 20230222084145_Initial 20230222083702_CreateRlsEntities -i -o DeploymentScripts/RollbackInitial.sql
