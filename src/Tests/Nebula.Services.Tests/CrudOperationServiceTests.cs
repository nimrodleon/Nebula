using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Nebula.Data;
using Moq;

namespace Nebula.Services.Tests;

class TestDocument : IGenericDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Operaciones Crud para cualquier Clase.
/// </summary>
[TestClass]
public class CrudOperationServiceTests
{
    private readonly CrudOperationService<TestDocument> _service;

    public CrudOperationServiceTests()
    {
        Mock<IOptions<DatabaseSettings>> mockOptions = new();
        mockOptions.Setup(x => x.Value).Returns(new DatabaseSettings()
        {
            ConnectionString = "mongodb://localhost:27017",
            DatabaseName = "testDb"
        });
        _service = new CrudOperationService<TestDocument>(mockOptions.Object);
    }

    [TestMethod]
    public async Task CreateAsync_ShouldInsertDocument()
    {
        // Arrange
        var testDocument = new TestDocument { Name = "Test" };
        // Act
        var result = await _service.CreateAsync(testDocument);
        // Assert
        Assert.IsNotNull(result);
    }
}
