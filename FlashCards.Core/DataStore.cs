using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlashCards.Core.Model;
using Newtonsoft.Json;
using Windows.Storage;

namespace FlashCards.Core
{
    public class DataStore : IDataStore
    {
        private string _baseFolder;
        public DataStore()
        {
            _baseFolder = ApplicationData.Current.LocalFolder.Path;
        }

        public IEnumerable<CardCollection> GetCollections()
        {
            var collectionFiles = ListFiles(_baseFolder, "memcards");
            if (!collectionFiles.Any())
            {
                yield return new CardCollection
                {
                    Id = Guid.NewGuid(),
                    Name = "default",
                    Description = "Default collection"
                };
            }
            else
            {
                foreach (var file in collectionFiles)
                {
                    yield return GetCollection(file);
                }
            }
        }

        public CardCollection GetCollection(string fullPath)
        {
            string collectionJson = ReadFile(fullPath);
            if (collectionJson != null)
            {
                return JsonConvert.DeserializeObject<CardCollection>(collectionJson);
            }
            return null;
        }

        public void SaveCollection(CardCollection collection)
        {
            var collectionJson = JsonConvert.SerializeObject(collection);
            WriteFile(Path.Combine(_baseFolder, collection.FileName), collectionJson);
        }

        public virtual string ReadFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                return File.ReadAllText(fullPath);
            }
            return null;
        }
        public void RemoveCollection(CardCollection collection)
        {
            if (collection != null)
            {
                DeleteFile(Path.Combine(_baseFolder, collection.FileName));
            }

        }

        public virtual void WriteFile(string fullPath, string body)
        {
            File.WriteAllText(fullPath, body);
        }

        public virtual void DeleteFile(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public virtual IEnumerable<string> ListFiles(string parentPath, string extension)
        {
            return Directory.EnumerateFiles(parentPath, $"*.{extension}", SearchOption.TopDirectoryOnly);
        }

        public string GetFileName(Guid collectionId)
        {
            return $"{collectionId}.memcards";
        }
    }
}
