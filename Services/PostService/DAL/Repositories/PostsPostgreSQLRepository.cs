using Microsoft.EntityFrameworkCore;
using PostService.DAL.Models;

namespace PostService.DAL.Repositories
{
    public class PostsPostgreSQLRepository
    {
        private readonly PostgreSQLContext _context;

        public PostsPostgreSQLRepository(PostgreSQLContext context)
        {
            _context = context;
        }

        public async Task SaveMetadataAsync(PostMetadataModel post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task<PostMetadataModel?> GetMetadataAsync(string hash)
        {
            return await _context.Posts.FirstOrDefaultAsync(p => p.Hash == hash);
        }
    }
}
