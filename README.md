# EF support RLS migration scripts
This repo implements support for adding/modifiying RLS (Row level security) entities during EF migrations

1. Implemented 'ExtendedSqlServerMigrationsSqlGenerator' extended from EF's 'SqlServerMigrationsSqlGenerator'
2. 'Generate' method for 'CreateTableOperation'/'DropTableOperation' is overriden
3. Created first empty migration. Added sql scripts for creating pre-requisite rls related entities (predicate function + empty security policy).
4. Created custom attribute 'EnableRlsAttribute' and applied to required tables from dbcontext, which is read at the time of script generation to decide whether to add table filter/block predicate for table (or whether to drop predicates in case of drop table)

#Helper commands for script generation during deployment

dotnet ef migrations script 20230222083702_CreateRlsEntities 20230222084145_Initial -i -o DeploymentScripts/Initial.sql

dotnet ef migrations script 20230222084145_Initial 20230222083702_CreateRlsEntities -i -o DeploymentScripts/RollbackInitial.sql
