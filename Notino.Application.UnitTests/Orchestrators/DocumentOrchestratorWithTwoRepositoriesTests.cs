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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Notino.Application.UnitTests.Orchestrators
{
    public class DocumentOrchestratorWithTwoRepositoriesTests    
    {

        private readonly Mock<IDocumentRepository> _mockDocumentRepoFirst;
        private readonly Mock<IDocumentRepository> _mockDocumentRepoSecond;
        private readonly IDocumentPersistenceOrchestrator _documentPersistenceOrchestrator;

        public DocumentOrchestratorWithTwoRepositoriesTests()
        {
            _mockDocumentRepoFirst = MockDocumentRepository.GetDocumentRepository();
            _mockDocumentRepoSecond = MockDocumentRepository.GetDocumentRepository();

            var mockLogger = new Mock<ILogger<DocumentPersistenceOrchestrator>>();
            _documentPersistenceOrchestrator = new DocumentPersistenceOrchestrator(
                new List<IDocumentRepository>() 
                {
                    _mockDocumentRepoFirst.Object,
                    _mockDocumentRepoSecond.Object
                },                
                mockLogger.Object
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
                DocumentTag = new List<DocumentTag>()
            };

            var documentTags = new List<string>() { "tag" };

            await _documentPersistenceOrchestrator
                .AddAsync(documentToAdd, documentTags, CancellationToken.None);

            
            var documentFirstRepo = await _mockDocumentRepoFirst.Object.GetByIdAsync("3", CancellationToken.None);
            var documentSecondRepo = await _mockDocumentRepoSecond.Object.GetByIdAsync("3", CancellationToken.None);

            documentFirstRepo.ShouldNotBeNull();
            documentFirstRepo.ShouldBeOfType<Document>();
            documentFirstRepo.Id.ShouldBe(documentFirstRepo.Id);
            documentFirstRepo.RawJson.ShouldBe(documentToAdd.RawJson);
            _mockDocumentRepoFirst.Verify(
                m => m.AddDocumentWithTagsAsync(documentToAdd, documentTags, CancellationToken.None, false),
                Times.Once
            );

            documentSecondRepo.ShouldNotBeNull();
            documentSecondRepo.ShouldBeOfType<Document>();
            documentSecondRepo.Id.ShouldBe(documentFirstRepo.Id);
            documentSecondRepo.RawJson.ShouldBe(documentToAdd.RawJson);
        }

        [Fact]
        public async Task Add_ValidDocumentOneRepositoryThrowsException_DocumentNotAdded()
        {
            var documentToAdd = new Document()
            {
                Id = "3",
                RawJson = @"{""Tags"":[""a"",""b""],""Data"":{""some"":""data"",""optional"":""fields""},""Id"":""2""}",
                DocumentTag = new List<DocumentTag>()
            };
            
            _mockDocumentRepoSecond.Setup(r =>
                r.AddDocumentWithTagsAsync(
                    It.IsAny<Document>(),
                    It.IsAny<IEnumerable<string>>(), 
                    It.IsAny<CancellationToken>(), It.IsAny<bool>())
                ).ThrowsAsync(new Exception());

            
            var result = await Should.ThrowAsync<Exception>(async ()
                => await _documentPersistenceOrchestrator.AddAsync(
                 documentToAdd,
                 new List<string>() { },
                 CancellationToken.None
             ));

            var documentFirstRepo = await _mockDocumentRepoFirst
                .Object
                .GetByIdAsync(documentToAdd.Id, CancellationToken.None);

            var documentSecondRepo = await _mockDocumentRepoSecond.
                Object
                .GetByIdAsync(documentToAdd.Id, CancellationToken.None);

            documentFirstRepo.ShouldBeNull();
            _mockDocumentRepoFirst.Verify(
                m => m.DeleteDocumentWithTagsAsync(documentToAdd.Id, CancellationToken.None),
                Times.Once
            );

            documentSecondRepo.ShouldBeNull();            
        }


        #endregion
    }

}
