using EntityFrameworkRls.Helpers;
using EntityFrameworkRls.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;

namespace EntityFrameworkRls
{
    public class PtDbContext : DbContext
    {
        IHttpContextAccessor _httpContextAccessor;
        public PtDbContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var sqlConnection = new SqlConnection(@"Server=.;Database=Test;Integrated Security=SSPI;");
            // Set TenantId in SESSION_CONTEXT to TenantId
            // to enable Row-Level Security filtering.
            sqlConnection.StateChange += (sender, e) =>
            {
                if (e.CurrentState == ConnectionState.Open)
                {
                    var tenantId = GetTenantId();
                    var cmd = sqlConnection.CreateCommand();
                    cmd.CommandText = @"exec sp_set_session_context @key=N'TenantId', @value=@TenantId";
                    cmd.Parameters.AddWithValue("@TenantId", tenantId);
                    cmd.ExecuteNonQuery();
                }
            };

            optionsBuilder.UseSqlServer(sqlConnection);
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<TaxReturn> TaxReturns { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Pt");

            modelBuilder.Entity<Client>().ToTable(x => x.EnableRLS());
            //modelBuilder.Entity<TaxReturn>().ToTable(x => x.EnableRLS());
        }

        private Guid GetTenantId()
        {
            return Guid.Parse(_httpContextAccessor?.HttpContext?.Request?.Query["tenantId"].ToString());
        }
    }

    public class RlsPolicy : IAnnotation
    {
        public string Name => "EnableRls";
        public object? Value => true;
    }
}
