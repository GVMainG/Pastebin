using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.Services.Interfaces
{
    public interface IPostService
    {
        IEnumerable<Post> Get(Func<Post, bool> func);
        void Add(Post post);
        void Dispose();
    }
}
