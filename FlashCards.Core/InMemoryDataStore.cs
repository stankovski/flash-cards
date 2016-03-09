using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Core.Model;
using Newtonsoft.Json;

namespace FlashCards.Core
{
    public class InMemoryDataStore : DataStore
    {
        Dictionary<string, string> _dataStore = new Dictionary<string, string>();

        public override string ReadFile(string fullPath)
        {
            if (_dataStore.ContainsKey(fullPath))
            {
                return _dataStore[fullPath];
            }
            return null;
        }

        public override void WriteFile(string fullPath, string body)
        {
            _dataStore[fullPath] = body;
        }

        public override IEnumerable<string> ListFiles(string parentPath, string extension)
        {
            return _dataStore.Keys.Where(k => k.EndsWith(extension)).Select(k => Path.GetFileNameWithoutExtension(k));
        }        
    }
}
