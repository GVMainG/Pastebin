using Microsoft.EntityFrameworkCore;
using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.DAL
{
    public class PostgreSQLContext : DbContext
    {
        /// <summary>
        /// Строка подключения для БД.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Таблица Posts.
        /// </summary>
        public virtual DbSet<Post> Posts => Set<Post>();

        /// <summary>
        /// Конструктор для подключения БД по строке подключения.
        /// </summary>
        /// <param name="connectionString"></param>
        public PostgreSQLContext(string connectionString)
        {
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
