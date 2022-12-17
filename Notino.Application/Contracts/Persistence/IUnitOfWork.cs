using System.Threading.Tasks;

namespace Notino.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        IDocumentRepository DocumentRepository { get; }        

        /*
         In current solution this method maybe breaks LSP principle. 
         Particularly, in HDD based persistence implementaction.         
         */
        Task SaveAsync();
    }
}
