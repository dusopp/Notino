using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notino.Application.Contracts.Persistence
{
    public interface IUnitOfWork
    {
        IDocumentRepository DocumentRepository { get; }        

        Task Save();
    }
}
