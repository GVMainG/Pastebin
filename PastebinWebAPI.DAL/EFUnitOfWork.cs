using PastebinWebAPI.DAL.Models;
using PastebinWebAPI.DAL.Repositories;
using PastebinWebAPI.DAL.Repositories.PostgreSQL;



namespace PastebinWebAPI.DAL
{
    public class EFUnitOfWork(PostgreSQLContext db) : IUnitOfWork
    {
        private bool disposed;
        private readonly PostgreSQLContext _context = db;

        private PostRepositoryPostgreSQL postRepository;

        public IRepository<Post> Posts
        {
            get
            {
                if (postRepository == null)
                    postRepository = new PostRepositoryPostgreSQL(_context);

                return postRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
