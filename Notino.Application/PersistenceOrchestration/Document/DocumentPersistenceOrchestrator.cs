using Microsoft.Extensions.Logging;
using Notino.Domain.Contracts.Persistence;
using Notino.Domain.Contracts.PersistenceOrchestration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Notino.Application.PersistenceOrchestration.Document
{
    public class DocumentPersistenceOrchestrator : IDocumentPersistenceOrchestrator
    {
        private readonly List<IDocumentRepository> _documentRepositories;
        private readonly ILogger<DocumentPersistenceOrchestrator> _logger;

        public DocumentPersistenceOrchestrator(            
            IEnumerable<IDocumentRepository> docRepos, ILogger<DocumentPersistenceOrchestrator> logger
            )
        {
            if (docRepos == null)
                throw new ArgumentNullException(nameof(docRepos));

            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _documentRepositories = new List<IDocumentRepository>();
            _logger = logger;

            foreach (var repo in docRepos)            
                _documentRepositories.Add(repo);
            
            
        }

        public async Task AddAsync(Domain.Entities.Document document, IEnumerable<string> tagNames, CancellationToken ct)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (tagNames == null)
                throw new ArgumentNullException(nameof(tagNames));


            var createTasks = new List<Task<Domain.Entities.Document>>();
            var revertFuncs = new Dictionary<int, Func<string, CancellationToken, Task<Domain.Entities.Document>>>();
            
            
            for (int i = 0; i < _documentRepositories.Count; i++)
            {
                createTasks.Add(_documentRepositories[i]
                    .AddDocumentWithTagsAsync(document, tagNames, ct));
                revertFuncs.Add(i, _documentRepositories[i].DeleteDocumentWithTagsAsync);
            }

            Task aggregationTask = Task.WhenAll(createTasks);

            try
            {                           
                await aggregationTask;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error adding document: Id:{document.Id}, RawJson: {document.RawJson}");
                
                if (aggregationTask?.Exception?.InnerExceptions != null && 
                    aggregationTask.Exception.InnerExceptions.Any())
                foreach (var innerEx in aggregationTask.Exception.InnerExceptions)               
                    _logger.LogError(innerEx, "AggregateException");               


                await RevertAsync(createTasks, document.Id, revertFuncs, ct);
                
                throw;
            }                   
        }

        /*     
            For future consideration, moving this method to common BaseOrchestratorClass.   
            Now premature optimisation
        */
        public async Task RevertAsync(
            List<Task<Domain.Entities.Document>> failedTasks, 
            string id, 
            Dictionary<int, Func<string, CancellationToken, Task<Domain.Entities.Document>>> revertFuncs, 
            CancellationToken ct, 
            int revertsCnt = 0)
        {
            var revertMethodsDict = new Dictionary<int, Func<string, CancellationToken, Task<Domain.Entities.Document>>>();
            var revertTasks = new List<Task<Domain.Entities.Document>>();            

            
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

            Task aggregationTask = Task.WhenAll(revertTasks);
            try
            {
                await aggregationTask;
            }
            catch(Exception ex)
            {
                if (revertsCnt > 2)
                {
                    _logger.LogError(ex, $"Error reverting document: Id:{id}");

                    if (aggregationTask?.Exception?.InnerExceptions != null &&
                        aggregationTask.Exception.InnerExceptions.Any())
                        foreach (var innerEx in aggregationTask.Exception.InnerExceptions)
                            _logger.LogError(innerEx, "AggregateException");

                    throw;
                }
                    

                await RevertAsync(revertTasks, id, revertFuncs, ct, ++revertsCnt);
            }
        }

        public async Task UpdateAsync(
            Domain.Entities.Document document,
            IEnumerable<string> tagNames,
            CancellationToken ct)
        {
            if (document == null)
                throw new ArgumentNullException(nameof(document));

            if (tagNames == null)
                throw new ArgumentNullException(nameof(tagNames));

            var updateTasks = new List<Task<Domain.Entities.Document>>();
            var revertFuncs = new Dictionary<int, Func<string, CancellationToken, Task<Domain.Entities.Document>>>();

            for (int i = 0; i < _documentRepositories.Count; i++)
            {
                updateTasks.Add(_documentRepositories[i]
                    .UpdateDocumentWithTagsAsync(document, tagNames, ct));
                revertFuncs.Add(i, _documentRepositories[i].DeleteDocumentWithTagsAsync);
            }

            var aggregationTask = Task.WhenAll(updateTasks);

            try
            {
                await aggregationTask;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"Error adding document: Id:{document.Id}, RawJson: {document.RawJson}");

                if (aggregationTask?.Exception?.InnerExceptions != null &&
                    aggregationTask.Exception.InnerExceptions.Any())
                    foreach (var innerEx in aggregationTask.Exception.InnerExceptions)
                        _logger.LogError(innerEx, "AggregateException");

                await RevertAsync(updateTasks, document.Id, revertFuncs, ct);
                throw;
            }            
        }
    }
}
