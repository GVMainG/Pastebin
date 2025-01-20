using Microsoft.EntityFrameworkCore;
using PostService.DAL.Models;

namespace PostService.DAL.Repositories
{
    /// <summary>
    /// Репозиторий для работы с метаданными постов в PostgreSQL.
    /// </summary>
    public class PostsPostgreSQLRepository
    {
        private readonly PostgreSQLContext _context;

        public PostsPostgreSQLRepository(PostgreSQLContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Сохранение метаданных поста.
        /// </summary>
        /// <param name="post">Модель метаданных поста.</param>
        public async Task SaveMetadataAsync(PostMetadataModel post)
        {
            if (post == null) throw new ArgumentNullException(nameof(post));
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Получение метаданных поста по хэшу.
        /// </summary>
        /// <param name="hash">Хэш поста.</param>
        /// <returns>Модель метаданных поста.</returns>
        public async Task<PostMetadataModel?> GetMetadataAsync(string hash)
        {
            if (string.IsNullOrEmpty(hash)) throw new ArgumentException("Hash cannot be null or empty.", nameof(hash));
            return await _context.Posts.FirstOrDefaultAsync(p => p.Hash == hash);
        }
    }
}
