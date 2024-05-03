using PastebinWebAPI.DAL.Models;
using PastebinWebAPI.DAL.Repositories;



namespace PastebinWebAPI.DAL
{
    /// <summary>
    /// Интерфейс для объявления шаблона UnitOfWork.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        public IRepository<Post> Posts { get; }

        public void Save();
    }
}
