using Microsoft.Extensions.Options;
using Notino.Application.Contracts.Persistence;
using Notino.Application.Contracts.PersistenceOrchestration;
using Notino.Application.Settings;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.PersistenceOrchestration.Document
{
    public class DocumentPersistenceOrchestrator : IDocumentPersistenceOrchestrator
    {
        private readonly List<IDocumentRepository> _documentRepositories;
        private readonly IUnitOfWork unitOfWork;

        public DocumentPersistenceOrchestrator(
            //IOptions<PersistenceSettings> options,
            IEnumerable<IDocumentRepository> docRepos, 
            IUnitOfWork unitOfWork)
        {
            if (docRepos == null)
                throw new ArgumentNullException(nameof(docRepos));

            this.unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            //var settings = options.Value;
            _documentRepositories = new List<IDocumentRepository>();
 
            foreach (var repo in docRepos)            
                _documentRepositories.Add(repo);
        }

        public async Task AddAsync(Domain.Document document, IEnumerable<string> tagNames, CancellationToken ct)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (tagNames == null)
                throw new ArgumentNullException(nameof(tagNames));


            var createTasks = new List<Task>();
            var revertFuncs = new Dictionary<int, Func<string, CancellationToken, Task>>();

            for (int i = 0; i < _documentRepositories.Count; i++)
            {
                createTasks.Add(_documentRepositories[i]
                    .AddDocumentWithTagsAsync(document, tagNames, ct));
                revertFuncs.Add(i, _documentRepositories[i].DeleteDocumentWithTagsAsync);
            }

          
            var result = Task.WhenAll(createTasks);

            try
            {
                await result;
            }
            catch
            {                
                await RevertAsync(createTasks, document.Id, revertFuncs, ct);
                throw;
            }

            await unitOfWork.SaveAsync();            
        }

        /*     
            For future consideration, moving this method to common BaseOrchestratorClass.   
            Now premature optimisation
        */
        public async Task RevertAsync(List<Task> failedTasks, string id, Dictionary<int, Func<string, CancellationToken, Task>> revertFuncs, CancellationToken ct, int revertsCnt = 0)
        {
            var revertMethodsDict = new Dictionary<int, Func<string, CancellationToken, Task>>();
            var revertTasks = new List<Task>();

            for (int i = 0; i < failedTasks.Count; i++)
            {
                if (!failedTasks[i].IsFaulted)
                {
                    revertMethodsDict.Add(i, revertFuncs[i]);
                    revertTasks.Add(revertFuncs[i](id, ct));
                }
                else
                {
                    revertFuncs.Remove(i);
                }
            }

            var result = Task.WhenAll(revertTasks);
            try
            {
                await result;
            }
            catch
            {
                if (revertsCnt > 3)
                    throw;

                await RevertAsync(revertTasks, id, revertFuncs, ct, ++revertsCnt);
            }
        }

        public async Task UpdateAsync(Domain.Document document, IEnumerable<string> tagNames, CancellationToken ct)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (tagNames == null)
                throw new ArgumentNullException(nameof(tagNames));

            var updateTasks = new List<Task>();
            var revertFuncs = new Dictionary<int, Func<string, CancellationToken, Task>>();

            for (int i = 0; i < _documentRepositories.Count; i++)
            {
                updateTasks.Add(_documentRepositories[i]
                    .UpdateDocumentWithTagsAsync(document, tagNames, ct));
                revertFuncs.Add(i, _documentRepositories[i].DeleteDocumentWithTagsAsync);
            }

            var result = Task.WhenAll(updateTasks);

            try
            {
                await result;
            }
            catch
            {
                await RevertAsync(updateTasks, document.Id, revertFuncs, ct);
                throw;
            }

            await unitOfWork.SaveAsync();
        }
    }
}
