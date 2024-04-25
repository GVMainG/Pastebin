using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.DAL.Repositories
{
    public interface IRepository<Model>  where Model : ModelBase
    {
        public Model Get(Guid id);

        public IEnumerable<Model> Get()
        {
            return Get(x => true);
        }

        public IEnumerable<Model> Get(Func<Model, bool> predicate);

        public void Create(Model item);

        public void Update(Model item);

        public void Delete(Guid id);
    }
}
