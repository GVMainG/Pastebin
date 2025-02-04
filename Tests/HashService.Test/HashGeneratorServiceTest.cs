using HashService.BL.Services;

namespace HashService.Test
{
    public class HashGeneratorServiceTest
    {
        [Fact]
        public void GenerateHashTestNotNull()
        {
            // Arrange.
            var hashGeneratorService = new HashGeneratorService();

            // Act.
            var hash = hashGeneratorService.GenerateHash();

            // Assert.
            Assert.NotNull(hash);
        }

        [Fact]
        public void GenerateHashTestNotEmpty()
        {
            // Arrange.
            var hashGeneratorService = new HashGeneratorService();

            // Act.
            var hash = hashGeneratorService.GenerateHash();

            // Assert.
            Assert.NotEmpty(hash ?? string.Empty);
        }

        [Fact]
        public void GenerateHashTestLength()
        {
            // Arrange.
            var hashGeneratorService = new HashGeneratorService();

            // Act.
            var hash = hashGeneratorService.GenerateHash();         

            // Assert.
            Assert.True((hash?.Length ?? 0) == HashGeneratorService.HashLength);
        }

        [Fact]
        public void GenerateHashTestUniqueValue()
        {
            // Arrange.
            var hashGeneratorService = new HashGeneratorService();

            // Act.
            List<string> hashs = [];
            for (int i = 0; i < 1000; i++)
            {
                hashs.Add(hashGeneratorService.GenerateHash());
            }

            // Assert.
            Assert.True(hashs.GroupBy(x => x).Count() == hashs.Count);
        }
    }
}
