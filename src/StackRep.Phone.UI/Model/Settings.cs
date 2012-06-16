using System.IO;
using System.IO.IsolatedStorage;
using Newtonsoft.Json;

namespace StackRep.Phone.UI.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Settings : ISettings
    {
        private const string Path = "Settings.dat";

        public Settings()
        {
            ReputationHistory = 30;
        }

        public void Load()
        {
            using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storageFile.FileExists(Path))
                    return;

                string rawData;
                using (var reader = new StreamReader(new IsolatedStorageFileStream(Path, FileMode.Open, storageFile)))
                {
                    rawData = reader.ReadToEnd();
                }

                var settings = JsonConvert.DeserializeObject<Settings>(rawData);

                ReputationHistory = settings.ReputationHistory;
            }
        }

        public void Save()
        {
            using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = new IsolatedStorageFileStream(Path, FileMode.Create, storageFile))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(JsonConvert.SerializeObject(this));
            }
        }

        [JsonProperty(PropertyName = "reputation_history")]
        public int ReputationHistory { get; set; }
    }
}
