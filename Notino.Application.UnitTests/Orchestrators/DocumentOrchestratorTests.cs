using Moq;
using Notino.Application.Contracts.Persistence;
using Notino.Application.PersistenceOrchestration.Document;
using Notino.Application.UnitTests.Mocks;
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
        private readonly DocumentPersistenceOrchestrator _documentPersistenceOrchestrator;

        public DocumentOrchestratorTests()
        {                      
            _mockDocumentRepo = MockDocumentRepository.GetDocumentRepository();           
            _documentPersistenceOrchestrator = new DocumentPersistenceOrchestrator(
                new List<IDocumentRepository>(){ _mockDocumentRepo.Object }               
            );           
        }

        [Fact]
        public async Task Add_ValidDocument_ShouldReturnTrue()
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


        [Fact]
        public async Task Update_ValidDocument_ShouldReturnTrue()
        {
            var documentToAdd = new Document()
            {
                Id = "2",
                RawJson = @"{""Tags"":[""test"",""b""],""Data"":{""some"":""data"",""optional"":""fields""},""Id"":""2""}",
                DocumentTag = new LinkedList<DocumentTag>()
            };

            //_mockDocumentRepo.Setup(r => r.);
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
    }
}
