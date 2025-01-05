using MongoDB.Driver;
using PostService.Data.Models;
using PostService.Data.Repositorys.Interfaces;

namespace PostService.Data.Repositorys
{
    public class PostRepository : IPostRepository
    {
        private readonly MainMongoDbContext _context;

        public PostRepository(MainMongoDbContext context)
        {
            _context = context;
        }

        public async Task<PostModel> Add(PostModel post)
        {
            await _context.TextBlocks.InsertOneAsync(post);

            return post;
        }

        public async Task<PostModel> Get(string id)
        {
            return await _context.TextBlocks.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> Delete(string id)
        {
            var result = await _context.TextBlocks.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
