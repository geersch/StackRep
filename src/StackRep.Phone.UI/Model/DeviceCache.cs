using System;
using System.IO;
using System.IO.IsolatedStorage;
using Newtonsoft.Json;

namespace StackRep.Phone.UI.Model
{
    public class FakeDeviceCache : IDeviceCache
    {
        public void Save(string key, object value)
        {
        }

        public T Load<T>(string key)
        {
            return default(T);
        }
    }

    public class DeviceCache : IDeviceCache
    {
        public void Save(string key, object value)
        {
            var path = String.Format("{0}.dat", key);
            using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = new IsolatedStorageFileStream(path, FileMode.Create, storageFile))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(JsonConvert.SerializeObject(value));
            }
        }

        public T Load<T>(string key)
        {
            var path = String.Format("{0}.dat", key);
            using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storageFile.FileExists(path))
                    return default(T);

                string rawData;
                using (var reader = new StreamReader(new IsolatedStorageFileStream(path, FileMode.Open, storageFile)))
                {
                    rawData = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<T>(rawData);
            }
        }
    }
}