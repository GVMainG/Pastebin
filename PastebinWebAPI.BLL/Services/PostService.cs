using PastebinWebAPI.BLL.Services.Interfaces;
using PastebinWebAPI.DAL;
using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.BLL.Services
{
    public class PostService(PostgreSQLContext db) : IPostService
    {
        private IUnitOfWork db { get; set; } = new EFUnitOfWork(db);

        public IEnumerable<Post> Get(Func<Post, bool> func)
        {
            return db.Posts.Get(func);
        }

        public void Add(Post post)
        {
            db.Posts.Create(post);
            Save();
        }

        public void Save()
        {
            db.Save();
        }
        
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
