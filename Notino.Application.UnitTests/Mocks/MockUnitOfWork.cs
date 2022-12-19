using Moq;
using Notino.Application.Contracts.Persistence;

namespace Notino.Application.UnitTests.Mocks
{
    public static class MockUnitOfWork
    {
        public static Mock<IUnitOfWork> GetUnitOfWork()
        {
            var mockUow = new Mock<IUnitOfWork>();
            var mockDocumentRepo = MockDocumentRepository.GetDocumentRepository();

            mockUow.Setup(r => r.DocumentRepository).Returns(mockDocumentRepo.Object);

            return mockUow;
        }
    }
}
