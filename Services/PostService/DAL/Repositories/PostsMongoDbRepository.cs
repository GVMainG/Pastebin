using MongoDB.Driver;
using PostService.DAL.Models;

namespace PostService.DAL.Repositories
{
    public class PostsMongoDbRepository
    {
        private readonly IMongoCollection<PostTextModel> _collection;

        public PostsMongoDbRepository(MongoDbContext context)
        {
            _collection = context.Posts;
        }

        public async Task SavePostAsync(PostTextModel post)
        {
            await _collection.InsertOneAsync(post);
        }

        public async Task<string?> GetPostAsync(string hash)
        {
            var post = await _collection.Find(p => p.Hash == hash).FirstOrDefaultAsync();
            return post?.Text;
        }
    }
}
