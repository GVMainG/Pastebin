using Microsoft.EntityFrameworkCore;
using PostService.DAL.Models;

namespace PostService.DAL
{
    public class PostgreSQLContext : DbContext
    {
        public PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        public DbSet<PostMetadataModel> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка ключа для модели PostMetadataModel.
            modelBuilder.Entity<PostMetadataModel>().HasKey(p => p.Hash);
            base.OnModelCreating(modelBuilder);
        }
    }
}
