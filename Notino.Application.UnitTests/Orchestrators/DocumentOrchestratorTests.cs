﻿using Moq;
using Notino.Application.Contracts.Persistence;
using Notino.Application.Contracts.PersistenceOrchestration;
using Notino.Application.Exceptions;
using Notino.Application.PersistenceOrchestration.Document;
using Notino.Application.UnitTests.Mocks;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
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
            var documentToAdd = new Domain.Document()
            {
                Id = "3",
                RawJson = @"{""Tags"":[""a"",""b""],""Data"":{""some"":""data"",""optional"":""fields""},""Id"":""2""}",
                DocumentTag = new LinkedList<Domain.DocumentTag>()
            };
           
            await _documentPersistenceOrchestrator
                .AddAsync(documentToAdd, new List<string>() { "tag" }, CancellationToken.None);

            var document = await _mockDocumentRepo.Object.GetByIdAsync("3", CancellationToken.None);
            
            document.ShouldNotBeNull();
            document.ShouldBeOfType<Domain.Document>();
            document.Id.ShouldBe(document.Id);
            document.RawJson.ShouldBe(documentToAdd.RawJson);
        }

        [Fact]
        public async Task Add_Document_IsNull_ThrowsArgumentNullException()
        {
            Domain.Document documentToAdd = null;

            var result = await Should.ThrowAsync<ArgumentNullException>(async () => await _documentPersistenceOrchestrator.AddAsync(
                 documentToAdd,
                 new List<string>() { },
                 CancellationToken.None
             ));

            result.Message.ShouldBe("Value cannot be null. (Parameter 'document')");          
        }


        [Fact]
        public async Task Add_TagNames_IsNull_ThrowsArgumentNullException()
        {
            var documentToAdd = new Domain.Document();

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
            var documentToAdd = new Domain.Document()
            {
                Id = "2",
                RawJson = @"{""Tags"":[""test"",""b""],""Data"":{""some"":""data"",""optional"":""fields""},""Id"":""2""}",
                DocumentTag = new LinkedList<Domain.DocumentTag>()
            };

            await _documentPersistenceOrchestrator
                .UpdateAsync(documentToAdd, new List<string>() { "tag" }, CancellationToken.None);

            var document = await _mockDocumentRepo.Object.GetByIdAsync("2", CancellationToken.None);


            document.ShouldNotBeNull();
            document.ShouldBeOfType<Domain.Document>();
            document.Id.ShouldBe(document.Id);
            document.RawJson.ShouldBe(documentToAdd.RawJson);
        }

        [Fact]
        public async Task Update_Document_IsNull_ThrowsArgumentNullException()
        {
            Domain.Document documentToAdd = null;

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
