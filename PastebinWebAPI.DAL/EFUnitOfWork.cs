using PastebinWebAPI.DAL.Models;
using PastebinWebAPI.DAL.Repositories;
using PastebinWebAPI.DAL.Repositories.PostgreSQL;



namespace PastebinWebAPI.DAL
{
    public class EFUnitOfWork : UnitOfWorkBase
    {
        private readonly PostgreSQLContext _context;

        private PostRepositoryPostgreSQL postRepository;

        public EFUnitOfWork(string connectionString)
        {
            _context = new PostgreSQLContext(connectionString);
        }

        public RepositoryBase<Post> Posts
        {
            get
            {
                if (postRepository == null)
                    postRepository = new PostRepositoryPostgreSQL(_context);

                return postRepository;
            }
        }
        public override void Save()
        {
            _context.SaveChanges();
        }

        public override void Dispose(bool disposing)
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

        public override void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
