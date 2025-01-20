using MongoDB.Driver;
using PostService.DAL.Models;

namespace PostService.DAL.Repositories
{
    /// <summary>
    /// Репозиторий для работы с текстами постов в MongoDB.
    /// </summary>
    public class PostsMongoDbRepository
    {
        private readonly IMongoCollection<PostTextModel> _collection;

        public PostsMongoDbRepository(MongoDbContext context)
        {
            _collection = context?.Posts ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Сохранение текста поста.
        /// </summary>
        /// <param name="post">Модель текста поста.</param>
        public async Task SavePostAsync(PostTextModel post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));
            await _collection.InsertOneAsync(post);
        }

        /// <summary>
        /// Получение текста поста по хэшу.
        /// </summary>
        /// <param name="hash">Хэш поста.</param>
        /// <returns>Текст поста.</returns>
        public async Task<string?> GetPostAsync(string hash)
        {
            if (string.IsNullOrEmpty(hash)) throw new ArgumentException("Hash cannot be null or empty.", nameof(hash));
            var post = await _collection.Find(p => p.Hash == hash).FirstOrDefaultAsync();
            return post?.Text;
        }
    }
}
