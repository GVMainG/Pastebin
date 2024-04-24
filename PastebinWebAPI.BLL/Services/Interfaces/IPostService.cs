using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.BLL.Services.Interfaces
{
    public interface IPostService
    {
        IEnumerable<Post> Get(Func<Post, bool> func);
        void Add(Post post);
        void Dispose();
    }
}
