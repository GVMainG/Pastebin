using Microsoft.EntityFrameworkCore;
using PastebinWebAPI.DAL.Models;
using PastebinWebAPI.DAL.Repositories;



namespace PastebinWebAPI.DAL
{
    public abstract class UnitOfWorkBase : IDisposable
    {
        protected bool disposed = false;

        public RepositoryBase<Post> Posts { get; }

        public UnitOfWorkBase() { }

        public UnitOfWorkBase(string connectionString) { }

        public abstract void Save();

        public abstract void Dispose(bool disposing);

        public abstract void Dispose();
    }
}
