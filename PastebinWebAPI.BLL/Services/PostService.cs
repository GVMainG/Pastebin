using PastebinWebAPI.BLL.Services.Interfaces;
using PastebinWebAPI.DAL;
using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.BLL.Services
{
    public class PostService : IPostService
    {
        private UnitOfWorkBase db { get; set; }

        //public PostService(IUnitOfWork uow)
        //{
        //    db = uow;
        //}

        public PostService(string connectionString)
        {
            db = new EFUnitOfWork(connectionString);
        }

        public IEnumerable<Post> Get(Func<Post, bool> func)
        {
            return db.Posts.Get(func);
        }

        public void Dispose()
        {
            db.Dispose();
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
    }
}
