namespace HashService.Business.Services.Interfaces
{
    public interface IHashService
    {
        Task<string> GetHashAsync();
        Task GenerateHashesAsync(int count);
    }
}
