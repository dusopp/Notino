﻿using Moq;
using Notino.Application.Contracts.Persistence;
using Notino.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.UnitTests.Mocks
{
    public static class MockDocumentRepository
    {
        public static Mock<IDocumentRepository> GetDocumentRepository()
        {
            var documents = new List<Domain.Document>
            {
                new Domain.Document
                {
                    Id = "1",
                    RawJson = @"{""Tags"":[""h"",""f""],""Data"":{""some"":""data"",""optional"":""fields""},""Id"":""1""}",
                    DocumentTag = new LinkedList<Domain.DocumentTag>()
                },
                new Domain.Document
                {
                    Id = "2",
                    RawJson = @"{""Tags"":[""a"",""b""],""Data"":{""some"":""data"",""optional"":""fields""},""Id"":""2""}",
                    DocumentTag = new LinkedList<Domain.DocumentTag>()
                }
            };

            var mockRepo = new Mock<IDocumentRepository>();

            mockRepo.Setup(r =>
                r.AddDocumentWithTagsAsync(It.IsAny<Domain.Document>(), It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>(), It.IsAny<bool>()))
                .ReturnsAsync((Domain.Document document, IEnumerable<string> tagnames, CancellationToken ct, bool test) => {
                    documents.Add(document);
                    return document;
                });

            mockRepo.Setup(r =>
                r.UpdateDocumentWithTagsAsync(It.IsAny<Domain.Document>(), It.IsAny<IEnumerable<string>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Document document, IEnumerable<string> tagnames, CancellationToken ct) => {
                    var index = documents.FindIndex(d => d.Id == document.Id);
                    documents.RemoveAt(index);
                    documents.Add(document);

                    return document;
                });

            mockRepo.Setup(r =>
                r.GetByIdAsync(It.IsAny<string>(), CancellationToken.None))
                .ReturnsAsync((string id, CancellationToken ct) => {
                    var index = documents.FindIndex(d => d.Id == id);                 
 
                    return documents[index];
                });

            mockRepo.Setup(r =>
                r.DeleteDocumentWithTagsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns((Domain.Document document, CancellationToken ct) => {
                  var index = documents.FindIndex(d => d.Id == document.Id);
                  documents.RemoveAt(index);

                  return Task.FromResult(document.Id);
              });

            return mockRepo;

        }
    }
}
