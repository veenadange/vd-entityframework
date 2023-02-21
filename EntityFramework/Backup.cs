//using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Microsoft.EntityFrameworkCore.Metadata;
//using Microsoft.EntityFrameworkCore.Migrations.Design;
//using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
//using Microsoft.EntityFrameworkCore.Migrations;
//using Microsoft.VisualBasic;

//namespace EntityFramework
//{

// var dd = model?.GetAnnotations();
// var ss = model?.GetEntityTypes()?.FirstOrDefault()?.GetAnnotations();
//    public class Backup
//    {
//        public class DesignTimeServices : IDesignTimeServices
//        {
//            public void ConfigureDesignTimeServices(IServiceCollection services)
//            {
//                services.AddSingleton<IMigrationsCodeGenerator, CSharpMigrationsGenerator>();
//                services.AddSingleton<ICSharpMigrationOperationGenerator, CSharpMigrationOperationGenerator>();
//            }
//        }
//        public static class EntityTypeBuilderExtensions
//        {
//            public static EntityTypeBuilder<T> IsAudited<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<T>? configureOptions = null)
//               where T : class
//            {
//                entityTypeBuilder.AddDelayedAuditAnnotation(
//                    entityType =>
//                    {
//                        var auditName = $"{Constants.AnnotationPrefix}:{typeof(T).Name}";
//                        var audit = new Audit(auditName, auditTable, auditTrigger);
//                        entityTypeBuilder.adda(audit);
//                    });

//                return entityTypeBuilder;
//            }

//            private static EntityTypeBuilder<T> AddDelayedAuditAnnotation<T>(this EntityTypeBuilder<T> entityTypeBuilder, Action<IMutableEntityType> addAuditAction)
//               where T : class
//            {
//                entityTypeBuilder.GetEntityType().AddAnnotation(Constants.AddAuditAnnotationName, addAuditAction);

//                return entityTypeBuilder;
//            }

//            private static IMutableEntityType GetEntityType<T>(this EntityTypeBuilder<T> entityTypeBuilder) where T : class
//            {
//                var entityType = entityTypeBuilder.Metadata.Model.FindEntityType(typeof(T).FullName!);
//                if (entityType == null)
//                {
//                    throw new InvalidOperationException("Entity type is missing from model");
//                }

//                return entityType;
//            }

//            private static EntityTypeBuilder<T> AddAuditAnnotation<T>(this EntityTypeBuilder<T> entityTypeBuilder, string audit) where T : class
//            {
//                var entityType = entityTypeBuilder.GetEntityType();
//                entityType.AddAnnotation(audit, audit.Serialize());
//                return entityTypeBuilder;
//            }
//        }
//        public static class MigrationBuilderExtensions
//        {
//            public static OperationBuilder<CreateAuditTriggerOperation> CreateAuditTrigger(
//                this MigrationBuilder migrationBuilder,
//                string auditedEntityTableName)
//            {
//                var operation = new CreateAuditTriggerOperation(auditedEntityTableName);
//                migrationBuilder.Operations.Add(operation);

//                return new OperationBuilder<CreateAuditTriggerOperation>(operation);
//            }
//        }
//        public static class Helpers
//        {
//            //    public class CreateUserOperation : MigrationOperation
//            //    {
//            //        public string Name { get; set; }
//            //        public string Password { get; set; }
//            //    }

//            //    public static OperationBuilder<CreateUserOperation> CreateUser(
//            //this MigrationBuilder migrationBuilder,
//            //string name,
//            //string password)
//            //    {
//            //        var operation = new CreateUserOperation { Name = name, Password = password };
//            //        migrationBuilder.Operations.Add(operation);

//            //        return new OperationBuilder<CreateUserOperation>(operation);
//            //    }
//        }

//    }
//}

//base.Generate(operation, model, builder);
//switch (operation)
//{
//    case CreateAuditTriggerOperation createAuditTriggerOperation:
//        //base.Generate(operation, model, builder);
//        _createAuditTriggerSqlGenerator.Generate(createAuditTriggerOperation, builder);
//        break;

//    //case CreateTableOperation createTableOperation:
//    //    //base.Generate(operation, model, builder);
//    //    Console.WriteLine("CreateTableOperation");
//    //    builder.AppendLine("Create table Clients(name varchar(20));");
//    //    builder.EndCommand();
//    //    break;

//    default:
//        //Console.WriteLine("default op");
//        base.Generate(operation, model, builder);
//        break;
//}
//base.Generate(createTableOperation, model, builder, terminate);
//builder.AppendLine("Create table testNew(name varchar(20));");
//builder.EndCommand();
//using (var writer = new StringWriter())
//{
//    //SetCreatedUtcColumn(createTableOperation.Columns);
//    writer.Write("Create table testNew(name varchar(20));");
//    Statement(writer.ToString());
//}

//builder
//    .AppendLine("Create table Clients(name varchar(20));")
//.AppendLine(Dependencies.SqlGenerationHelper.StatementTerminator);
//EndStatement(builder);

//using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
//using Microsoft.EntityFrameworkCore.Migrations.Operations;
//using Microsoft.EntityFrameworkCore.Migrations;

//public static class MigrationBuilderExtensions
//{
//    public static OperationBuilder<CreateAuditTriggerOperation> CreateAuditTrigger(
//        this MigrationBuilder migrationBuilder)
//    {
//        var operation = new CreateAuditTriggerOperation();
//        migrationBuilder.Operations.Add(operation);

//        return new OperationBuilder<CreateAuditTriggerOperation>(operation);
//    }
//}
//public class CreateAuditTriggerOperation : MigrationOperation
//{
//    //public CreateAuditTriggerOperation()
//    //{
//    //    //TableName = tableName;
//    //}
//    public string TableName { get; set; }
//}
//internal interface ICreateAuditTriggerSqlGenerator
//{
//    void Generate(CreateAuditTriggerOperation operation, MigrationCommandListBuilder builder);
//}
//internal class CreateAuditTriggerSqlGenerator : ICreateAuditTriggerSqlGenerator
//{
//    public void Generate(CreateAuditTriggerOperation operation, MigrationCommandListBuilder builder)
//    {
//        builder.Append("Create table Clients(name varchar(20));");

//        builder.EndCommand();
//    }
//}