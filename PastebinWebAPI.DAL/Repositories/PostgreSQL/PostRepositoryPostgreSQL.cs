using Microsoft.EntityFrameworkCore;
using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.DAL.Repositories.PostgreSQL
{
    internal class PostRepositoryPostgreSQL : IRepository<Post>
    {
        private readonly PostgreSQLContext db;

        public PostRepositoryPostgreSQL(PostgreSQLContext db)
        {
            this.db = db;
        }

        public void Create(Post item)
        {
            db.Posts.Add(item);
        }

        public void Delete(Guid id)
        {
            Post book = db.Posts.Find(id);

            if (book != null)
                db.Posts.Remove(book);
        }

        public Post Get(Guid id)
        {
            return db.Posts.Find(id);
        }

        public IEnumerable<Post> Get(Func<Post, bool> predicate)
        {
            return db.Posts.Where(predicate);
        }

        public void Update(Post item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
