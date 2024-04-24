using Microsoft.EntityFrameworkCore;
using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.DAL.Repositories.PostgreSQL
{
    internal class PostRepositoryPostgreSQL : RepositoryBase<Post>
    {
        private readonly PostgreSQLContext db;

        public PostRepositoryPostgreSQL(PostgreSQLContext db)
        {
            this.db = db;
        }

        public override void Create(Post item)
        {
            db.Posts.Add(item);
        }

        public override void Delete(Guid id)
        {
            Post book = db.Posts.Find(id);

            if (book != null)
                db.Posts.Remove(book);
        }

        public override Post Get(Guid id)
        {
            return db.Posts.Find(id);
        }

        public override IEnumerable<Post> Get(Func<Post, bool> predicate)
        {
            return db.Posts.Where(predicate);
        }

        public override void Update(Post item)
        {
            db.Entry(item).State = EntityState.Modified;
        }
    }
}
