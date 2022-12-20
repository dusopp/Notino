using Microsoft.Extensions.Logging;
using Moq;
using Notino.Application.PersistenceOrchestration.Document;
using Notino.Application.UnitTests.Mocks;
using Notino.Domain.Contracts.Persistence;
using Notino.Domain.Contracts.PersistenceOrchestration;
using Notino.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Notino.Application.UnitTests.Orchestrators
{
    public class DocumentOrchestratorTests
    {
       
        private readonly Mock<IDocumentRepository> _mockDocumentRepo;
        private readonly IDocumentPersistenceOrchestrator _documentPersistenceOrchestrator;

        public DocumentOrchestratorTests()
        {                      
            _mockDocumentRepo = MockDocumentRepository.GetDocumentRepository();
            var mockLogger = new Mock<ILogger<DocumentPersistenceOrchestrator>>();
            _documentPersistenceOrchestrator = new DocumentPersistenceOrchestrator(
                new List<IDocumentRepository>(){ _mockDocumentRepo.Object }, mockLogger.Object
            );           
        }

        #region Add Method Tests

        [Fact]
        public async Task Add_ValidDocument_DocumentAdded()
        {
            var documentToAdd = new Document()
            {
                Id = "3",
                RawJson = @"{""Tags"":[""a"",""b""],""Data"":{""some"":""data"",""optional"":""fields""},""Id"":""2""}",
                DocumentTag = new LinkedList<DocumentTag>()
            };
           
            await _documentPersistenceOrchestrator
                .AddAsync(documentToAdd, new List<string>() { "tag" }, CancellationToken.None);

            var document = await _mockDocumentRepo.Object.GetByIdAsync("3", CancellationToken.None);
            
            document.ShouldNotBeNull();
            document.ShouldBeOfType<Document>();
            document.Id.ShouldBe(document.Id);
            document.RawJson.ShouldBe(documentToAdd.RawJson);
        }

        [Fact]
        public async Task Add_DocumentIsNull_ThrowsArgumentNullException()
        {
            Document documentToAdd = null;

            var result = await Should.ThrowAsync<ArgumentNullException>(async () => await _documentPersistenceOrchestrator.AddAsync(
                 documentToAdd,
                 new List<string>() { },
                 CancellationToken.None
             ));

            result.Message.ShouldBe("Value cannot be null. (Parameter 'document')");          
        }


        [Fact]
        public async Task Add_TagNamesIsNull_ThrowsArgumentNullException()
        {
            var documentToAdd = new Document();

            var result = await Should.ThrowAsync<ArgumentNullException>(async () => await _documentPersistenceOrchestrator.AddAsync(
                 documentToAdd,
                 null,
                 CancellationToken.None
             ));

            result.Message.ShouldBe("Value cannot be null. (Parameter 'tagNames')");
        }

        #endregion

        #region Update Method Tests
        [Fact]
        public async Task Update_ValidDocument_DocumentUpdated()
        {
            var documentToAdd = new Document()
            {
                Id = "2",
                RawJson = @"{""Tags"":[""test"",""b""],""Data"":{""some"":""data"",""optional"":""fields""},""Id"":""2""}",
                DocumentTag = new LinkedList<DocumentTag>()
            };
            
            await _documentPersistenceOrchestrator
                .UpdateAsync(documentToAdd, new List<string>() { "tag" }, CancellationToken.None);

            var document = await _mockDocumentRepo.Object.GetByIdAsync("2", CancellationToken.None);


            document.ShouldNotBeNull();
            document.ShouldBeOfType<Document>();
            document.Id.ShouldBe(document.Id);
            document.RawJson.ShouldBe(documentToAdd.RawJson);
        }

        [Fact]
        public async Task Update_DocumentIsNull_ThrowsArgumentNullException()
        {
            Document documentToAdd = null;

            var result = await Should.ThrowAsync<ArgumentNullException>(async () 
                => await _documentPersistenceOrchestrator.UpdateAsync(
                 documentToAdd,
                 new List<string>() { },
                 CancellationToken.None
             ));

            result.Message.ShouldBe("Value cannot be null. (Parameter 'document')");
        }

        #endregion
    }
}
