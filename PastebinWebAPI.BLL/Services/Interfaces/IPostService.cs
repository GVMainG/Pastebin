using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.BLL.Services.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса по работе с постами.
    /// </summary>
    public interface IPostService
    {
        /// <summary>
        /// Получение постов по условию.
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        IEnumerable<Post> Get(Func<Post, bool> func);

        /// <summary>
        /// Создание поста.
        /// </summary>
        /// <param name="post"></param>
        void Create(Post post);

        void Dispose();
    }
}
