using Notino.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Notino.Application.Contracts.PersistenceOrchestration;
using Notino.Application.Contracts.Persistence;
using Notino.Application.PersistenceOrchestration.Common;
using Notino.Application.Exceptions;
using Microsoft.Extensions.Configuration;
using Notino.Application.Settings;
using Microsoft.Extensions.Options;
using Notino.Application.Factories;

namespace Notino.Application.PersistenceOrchestration
{
    public class DocumentPersistenceOrchestrator : PersistenceOrchestrator<string>, IDocumentPersistenceOrchestrator
    {
        private readonly List<IDocumentRepository> _documentRepositories;        

        public DocumentPersistenceOrchestrator(IOptions<PersistenceSettings> options)
        {
            var settings = options.Value;
            _documentRepositories = new List<IDocumentRepository>();

            foreach (var className in settings.NonDbDocumentRepos)
            {
                _documentRepositories.Add(CommonFactory.Get<IDocumentRepository>(className));                
            }

            foreach (var className in settings.NonDbDocumentRepos)
            {
                _documentRepositories.Add(CommonFactory.Get<IDocumentRepository>(className));
            }

            //_documentRepositories = documentRepositories.ToArray();
            //linkedRepositories = new LinkedList<IDocumentRepository>();

            //foreach (var repo in documentRepositories)
            //{
            //    linkedRepositories.AddLast(repo);
            //}
        }

        public async Task AddAsync(Document document, IEnumerable<string> tagNames)
        {
            var tasks = new List<Task<string>>();

            foreach (var repo in _documentRepositories)
            {
                tasks.Add(repo.AddDocumentWithTagsAsync(document, new List<string>()));
            }

            var result = Task.WhenAll(tasks);

            try
            {
                await result;
            }
            catch
            {
                await Revert(tasks, document.Id);

                throw;
            }
        }

        protected override async Task Revert(List<Task<string>> tasks, string id)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (!tasks[i].IsFaulted)
                {
                    await _documentRepositories[i].DeleteDocumentAsync(id);
                }
            }
        }
    }
}
