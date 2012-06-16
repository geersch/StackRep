using System;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;

namespace StackExchange.Api
{
    public class AccessToken
    {
        private const string Path = "access_token.txt";

        public static readonly Uri AuthorizationUri =
            new Uri("https://stackexchange.com/oauth/dialog?client_id=302&scope=no_expiry&redirect_uri=https://stackexchange.com/oauth/login_success");

        public string Token
        {
            get;
            set;
        }

        public bool IsValid
        {
            get
            {
                return !String.IsNullOrEmpty(Token);
            }
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

                Token = String.IsNullOrEmpty(rawData) ? null : rawData;
            }
        }

        public void Save()
        {
            using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            using (var stream = new IsolatedStorageFileStream(Path, FileMode.Create, storageFile))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(Token);
            }
        }

        public void Delete()
        {
            using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storageFile.FileExists(Path))
                    return;

                storageFile.DeleteFile(Path);
            }
        }

        public static string ParseAccessToken(Uri uri)
        {
            var url = uri.ToString();
            if (String.IsNullOrEmpty(url) || !url.Contains("access_token"))
                return null;

            url = url.Substring(url.IndexOf("#") + 1);

            var parts = url.Split('&');

            foreach (var part in parts)
            {
                var values = part.Split('=');
                if (values[0] != "access_token")
                    continue;

                return values[1];
            }

            return null;
        }
    }
}