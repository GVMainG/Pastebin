using PastebinWebAPI.DAL;
using PastebinWebAPI.DAL.Models;
using PastebinWebAPI.Services.Interfaces;



namespace PastebinWebAPI.Services
{
    public class PostService : IPostService
    {
        private IUnitOfWork db { get; set; }

        //public PostService(IUnitOfWork uow)
        //{
        //    db = uow;
        //}

        public PostService()
        {
            db = new EFUnitOfWork();
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
            try
            {
                db.Save();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Dispose();
            }
        }
    }
}
