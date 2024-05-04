using PastebinWebAPI.BLL.Services.Interfaces;
using PastebinWebAPI.DAL;
using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.BLL.Services
{
    /// <summary>
    /// Сервиса по работе с постами.
    /// </summary>
    /// <param name="db"></param>
    public class PostService(PostgreSQLContext db) : IPostService
    {
        private IUnitOfWork db { get; set; } = new EFUnitOfWork(db);

        public IEnumerable<Post> Get(Func<Post, bool> func)
        {
            return db.Posts.Get(func);
        }

        public void Create(Post post)
        {
            db.Posts.Create(post);
            Save();
        }

        private void Save()
        {
            db.Save();
        }
        
        public void Dispose()
        {
            db.Dispose();
        }
    }
}
