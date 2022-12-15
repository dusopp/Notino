using Microsoft.Extensions.Options;
using Notino.Application.Contracts.Persistence;
using Notino.Application.Contracts.PersistenceOrchestration;
using Notino.Application.PersistenceOrchestration.Common;
using Notino.Application.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notino.Application.PersistenceOrchestration.Document
{
    public class DocumentPersistenceOrchestrator : BasePersistenceOrchestrator<string>
    {
        private readonly List<IDocumentRepository> _documentRepositories;
        private readonly IUnitOfWork unitOfWork;

        public DocumentPersistenceOrchestrator(IOptions<PersistenceSettings> options,
            IEnumerable<IDocumentRepository> repos
            ,IUnitOfWork unitOfWork)
        {
            var settings = options.Value;
            _documentRepositories = new List<IDocumentRepository>();


            foreach (var repo in repos)
            {
                _documentRepositories.Add(repo);
            }

            this.unitOfWork = unitOfWork;
        }


        public override async Task AddAsync(Domain.Document document, IEnumerable<string> tagNames)
        {
            var createTasks = new List<Task>();
            var revertFuncs = new Dictionary<int, Func<string, Task>>();

            for (int i = 0; i < _documentRepositories.Count; i++)
            {
                createTasks.Add(_documentRepositories[i]
                    .AddDocumentWithTagsAsync(document, tagNames));
                revertFuncs.Add(i, _documentRepositories[i].DeleteDocumentWithTagsAsync);
            }

            //foreach (var repo in _documentRepositories)
            //{
                
            //}

            var result = Task.WhenAll(createTasks);

            try
            {
                await result;
            }
            catch
            {
                
                await RevertAsync(createTasks, document.Id,revertFuncs);

                throw;
            }

            await unitOfWork.Save();
        }

        //public override Task AddAsync(Domain.Document document, IEnumerable<string> tagNames)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override async Task Revert(List<Task<string>> tasks, string id, int revertCnt = 0)
        //{
        //    var revertTasks = new List<Task<string>>();

        //    for (int i = 0; i < tasks.Count; i++)
        //    {
        //        if (!tasks[i].IsFaulted)
        //        {
        //            revertTasks.Add(_documentRepositories[i].DeleteById(id)); 
        //        }
        //    }

        //    var result = Task.WhenAll(tasks);
        //    try
        //    {
        //        await result;
        //    }
        //    catch
        //    {
        //        if(revertCnt > 3)
        //            throw;

        //        await Revert(revertTasks, id, ++revertCnt);
        //    }
        //}


    }
}
