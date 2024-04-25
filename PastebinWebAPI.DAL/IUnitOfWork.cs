using PastebinWebAPI.DAL.Models;
using PastebinWebAPI.DAL.Repositories;



namespace PastebinWebAPI.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<Post> Posts { get; }

        public void Save();
    }
}
