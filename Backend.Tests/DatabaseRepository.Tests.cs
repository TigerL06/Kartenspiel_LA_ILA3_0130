using Microsoft.VisualStudio.TestTools.UnitTesting;
using Backend.Models;
using Backend.Repositories;
using Microsoft.Extensions.Configuration;

namespace Backend.Tests
{
    [TestClass]
    public class DatabaseRepositoryTests
    {
        private DatabaseRepository _repository;

        [TestInitialize]
        public void TestInitialize()
        {
            // Konfiguration f�r die MongoDB-Verbindung
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _repository = new DatabaseRepository(configuration);
        }

        [TestMethod]
        public void GetPlayerById_ShouldReturnCorrectPlayer()
        {
            // Arrange
            var expectedId = "67933f5806d57403c157b3d5";
            var expectedName = "Wurstk�nig42";

            // Act
            var result = _repository.GetPlayerById(expectedId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedId, result.Id);
            Assert.AreEqual(expectedName, result.Name);
        }
    }
}
