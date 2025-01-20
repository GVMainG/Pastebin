using PostService.BL.Models;

namespace PostService.BL.Services.Interfaces
{
    public interface IPostService
    {
        /// <summary>
        /// Создает новый пост.
        /// </summary>
        /// <param name="text">Текст поста.</param>
        /// <returns>Хэш созданного поста.</returns>
        Task<string> CreatePostAsync(string text);

        /// <summary>
        /// Получает пост по хэшу.
        /// </summary>
        /// <param name="hash">Хэш поста.</param>
        /// <returns>Ответ с данными поста.</returns>
        Task<PostResponse?> GetPostAsync(string hash);
    }
}
