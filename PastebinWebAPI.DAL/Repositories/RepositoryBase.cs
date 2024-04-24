using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.DAL.Repositories
{
    public abstract class RepositoryBase<Model>  where Model : ModelBase
    {
        public abstract Model Get(Guid id);

        public IEnumerable<Model> Get()
        {
            return Get(x => true);
        }

        public abstract IEnumerable<Model> Get(Func<Model, bool> predicate);

        public abstract void Create(Model item);

        public abstract void Update(Model item);

        public abstract void Delete(Guid id);
    }
}
