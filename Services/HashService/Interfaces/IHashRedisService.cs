namespace HashService.Interfaces
{
    public interface IHashRedisService
    {
        void Add(string hash);
        string Get();
        void PreloadHashes(int count, IHashGeneratorService generator);
    }
}