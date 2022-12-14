using System;
using System.Collections.Generic;
using System.Text;

namespace Notino.Application.Settings
{
    public class PersistenceSettings
    {
        public List<string> NonPrimaryPersistenceRepos { get; set; }

        public List<string> RDBMSDocumentRepos { get; set; }

        public List<string> NoSqlDocumentRepos { get; set; }
    }
}
