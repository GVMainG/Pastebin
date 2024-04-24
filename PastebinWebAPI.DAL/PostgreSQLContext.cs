using Microsoft.EntityFrameworkCore;
using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.DAL
{
    public class PostgreSQLContext : DbContext
    {
        private readonly string connectionString;

        public virtual DbSet<Post> Posts => Set<Post>();

        public PostgreSQLContext(string connectionString)
        {
            // "Host=db-pastebin-api;Port=5432;Username=admin;Password=admin;Database=pastebinDb;"
            this.connectionString = connectionString;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(connectionString);
        }
    }
}
