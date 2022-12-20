using Moq;
using Notino.Domain.Contracts.Persistence;
using Notino.Domain.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.UnitTests.Mocks
{
    public static class MockDocumentRepository
    {
        public static Mock<IDocumentRepository> GetDocumentRepository()
        {
            var documents = new List<Document>
            {
                new Document
                {
                    Id = "1",
                    RawJson = @"{""Tags"":[""h"",""f""],""Data"":{""some"":""data"",""optional"":""fields""},""Id"":""1""}",
                    DocumentTag = new LinkedList<DocumentTag>()
                },
                new Document
                {
                    Id = "2",
                    RawJson = @"{""Tags"":[""a"",""b""],""Data"":{""some"":""data"",""optional"":""fields""},""Id"":""2""}",
                    DocumentTag = new LinkedList<DocumentTag>()
                }
            };

            var mockRepo = new Mock<IDocumentRepository>();

            mockRepo.Setup(r =>
                r.AddDocumentWithTagsAsync(It.IsAny<Document>(), It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                .ReturnsAsync((Document document, IEnumerable<string> tagnames, CancellationToken ct, bool test) => {
                    documents.Add(document);
                    return document;
                });

            mockRepo.Setup(r =>
                r.UpdateDocumentWithTagsAsync(It.IsAny<Document>(), It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Document document, IEnumerable<string> tagnames, CancellationToken ct) => {
                    var index = documents.FindIndex(d => d.Id == document.Id);
                    documents.RemoveAt(index);
                    documents.Add(document);

                    return document;
                });

            mockRepo.Setup(r =>
                r.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((string id, CancellationToken ct) => {
                    var index = documents.FindIndex(d => d.Id == id);                 
 
                    return index > -1 ? documents[index] : null;
                });

            mockRepo.Setup(r =>
                r.DeleteDocumentWithTagsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((string id, CancellationToken ct) => {
                  var index = documents.FindIndex(d => d.Id == id);
                  var doc = documents[index];
                  documents.RemoveAt(index);

                  return doc;
              });

            return mockRepo;

        }
    }
}
