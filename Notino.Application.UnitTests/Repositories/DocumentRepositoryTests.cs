using Microsoft.EntityFrameworkCore;
using Notino.Application.Contracts.Persistence;
using Notino.Application.Exceptions;
using Notino.Persistence.MSSQL;
using Notino.Persistence.MSSQL.Repositories;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Notino.Application.UnitTests.Repositories
{
    public class DocumentRepositoryTests
    {
        private DbContextOptions<NotinoDbContext> _contextOptions;

        public DocumentRepositoryTests()
        {            
        }

        private NotinoDbContext GetContextWithData(string dbName) 
        {
            var _contextOptions = new DbContextOptionsBuilder<NotinoDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;

            var dbContext = new NotinoDbContext(_contextOptions);
            
                dbContext.Tags.Add(
                    new Domain.Tag()
                    {
                        Id = 1,
                        Name = "a"
                    }
                );

                dbContext.Tags.Add(
                    new Domain.Tag()
                    {
                        Id = 2,
                        Name = "b"
                    }
                );

                dbContext.Documents.Add(
                    new Domain.Document()
                    {
                        Id = "1",
                        RawJson = @"{""Tags"":[""a"",""b""],""Data"":{""some"":""data"",""optional"":""fields""},""Id"":""1""}",
                    }
                );

                dbContext.DocumentTags.Add(new Domain.DocumentTag { DocumentId = "1", TagId = 1 });
                dbContext.DocumentTags.Add(new Domain.DocumentTag { DocumentId = "1", TagId = 2 });
                dbContext.SaveChanges();

                return dbContext;
         }        

        [Fact]
        public async Task GetById_ExistingDocument_ShouldReturnDocument()
        {
            
            var documentId = "1";
  
            using (var context = GetContextWithData(Guid.NewGuid().ToString()))
            {
                IDocumentRepository docRepo = new DocumentRepository(context);
                var document = await docRepo.GetByIdAsync(documentId, CancellationToken.None);

                document.ShouldNotBeNull();
                document.Id.ShouldBe("1");
            }
        }

        [Fact]
        public async Task GetById_NotExistingDocument_ShouldReturnNull()
        {
            var documentId = "3";            
            using (var context = GetContextWithData(Guid.NewGuid().ToString()))
            {
                IDocumentRepository docRepo = new DocumentRepository(context);
                var document = await docRepo.GetByIdAsync(documentId, CancellationToken.None);

                document.ShouldBeNull();                
            }
        }

        [Fact]
        public async Task AddDocumentWithTagsAsync_NewDocument_ShouldReturnDocument()
        {
            var documentToAdd = new Domain.Document()
            {
                Id = "3",
                RawJson = @"{""Tags"":[""k"",""u""],""Data"":{""some"":""data"",""optional"":""fields""},""Id"":""3""}",
            };
            var tags = new List<string>() { "k", "u" };

            using (var context = GetContextWithData(Guid.NewGuid().ToString()))
            {
                IDocumentRepository docRepo = new DocumentRepository(context);
                var document = await docRepo.AddDocumentWithTagsAsync(                      
                      documentToAdd, tags, CancellationToken.None
                    );
                await context.SaveChangesAsync();

                var newlyAddedDoc = await docRepo.GetByIdAsync(documentToAdd.Id, CancellationToken.None);
                var tagsCount = await context.Tags.CountAsync();

                newlyAddedDoc.ShouldNotBeNull();
                newlyAddedDoc.Id.ShouldBe(documentToAdd.Id);
                newlyAddedDoc.RawJson.ShouldBe(documentToAdd.RawJson);

                tagsCount.ShouldBe(4);
            }
        }

        [Fact]
        public async Task AddDocumentWithTagsAsync_DocumentWithExistingId_ThrowsAlreadyExistsException()
        {
            var documentToAdd = new Domain.Document()
            {
                Id = "1"                
            };            

            using (var context = GetContextWithData(Guid.NewGuid().ToString()))
            {
                IDocumentRepository docRepo = new DocumentRepository(context);
            
                var result = await Should.ThrowAsync<AlreadyExistsException>(async () => await docRepo.AddDocumentWithTagsAsync(
                    documentToAdd,
                    new List<string>() { },
                    CancellationToken.None
                ));

                result.Message.ShouldBe($"Document with Id:'{documentToAdd.Id}' already exists");                
            }
        }

        [Fact]
        public async Task DeleteDocumentWithTagsAsync_ExistingDocument_ShouldReturnDocumentId()
        {
            var documentToRemove = new Domain.Document()
            {
                Id = "1",               
            };            

            using (var context = GetContextWithData(Guid.NewGuid().ToString()))
            {
                IDocumentRepository docRepo = new DocumentRepository(context);
                var document = await docRepo.DeleteDocumentWithTagsAsync(
                      documentToRemove.Id, CancellationToken.None
                    );
                await context.SaveChangesAsync();

                var getDoc = await docRepo.GetByIdAsync(documentToRemove.Id, CancellationToken.None);
                var docTagsCount = await context.DocumentTags.CountAsync();

                getDoc.ShouldBeNull();
                docTagsCount.ShouldBe(0);
            }
        }

    }
}
