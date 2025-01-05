using TextService.Data.Models;
using TextService.Models;
using TextService.Repositorys.Interfaces;
using TextService.Services.Interfaces;

namespace TextService.Services
{
    /// <summary>
    /// Сервис по работе с текстовыми блокмаи.
    /// </summary>
    public class TextService : ITextService
    {
        private readonly ITextRepository _textRepository;

        public TextService(ITextRepository textRepository)
        {
            _textRepository = textRepository;
        }

        // Создание текста
        public async Task<TextModel> CreateTextAsync(TextDto textDto)
        {
            var newText = new TextModel
            {
                Id = Guid.NewGuid().ToString(),
                Content = textDto.Content,
                CreatedAt = DateTime.UtcNow,
                ExpirationDate = textDto.ExpirationDate ?? DateTime.UtcNow.AddDays(7)
            };

            return await _textRepository.CreateTextAsync(newText);
        }

        // Получение текста по ID
        public async Task<TextModel> GetTextAsync(string id)
        {
            return await _textRepository.GetTextByIdAsync(id);
        }

        // Удаление текста
        public async Task<bool> DeleteTextAsync(string id)
        {
            return await _textRepository.DeleteTextAsync(id);
        }

        // Получение всех текстов
        public async Task<IEnumerable<TextModel>> GetPopularTextsAsync()
        {
            return await _textRepository.GetAllTextsAsync();
        }
    }
}
