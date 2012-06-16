using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using SharpCompress.Compressor;
using SharpCompress.Compressor.Deflate;

namespace StackExchange.Api
{
    public class StackExchangeApi
    {
        private const string ApiVersion = "2.0";
        private const string BaseUri = "https://api.stackexchange.com/" + ApiVersion;
        private readonly string _apiKey;
        private readonly string _accessToken;
        private const int Timeout = 30000;

        private class RequestTimeout : IDisposable
        {
            private readonly Timer _timer;
            private readonly int _timeout;
            private int _totalRunningTime;

            public RequestTimeout(HttpWebRequest request, int timeout)
            {
                _timeout = timeout;
                _timer = new Timer(CheckRequestStatus, request, 1000, 1000);
            }

            public void Dispose()
            {
                _timer.Dispose();
            }

            private void CheckRequestStatus(object state)
            {
                var request = (HttpWebRequest)state;

                _totalRunningTime += 1000;

                if (_totalRunningTime > _timeout)
                {
                    request.Abort();
                }
            }
        }

        public event EventHandler<StackExchangeErrorEventArgs> Error;

        public StackExchangeApi(string apiKey, string accessToken)
        {
            _apiKey = apiKey;
            _accessToken = accessToken;
        }

        public delegate void StackExchangeApiCallback<T>(T data) where T : class, new();

        public void GetUser(StackExchangeApiCallback<User> callback)
        {
            GetStackExchangeObject("/me?filter=!-q2RdW51", callback);
        }

        public void GetReputationChanges(int days, StackExchangeApiCallback<List<ReputationChange>> callback)
        {
            var fromDate = DateTime.Now.AddDays(Math.Abs(days)*-1);
            var toDate = DateTime.Now;

            GetReputationChanges(fromDate, toDate, callback);
        }

        public void GetReputationChanges(DateTime fromDate, DateTime toDate, StackExchangeApiCallback<List<ReputationChange>> callback)
        {
            var path = String.Format("/me/reputation?fromdate={0}&todate={1}&filter=!-q2RdjCJ",
                fromDate.ToUnixTime(), toDate.ToUnixTime());

            GetStackExchangeObjects(path, callback);
        }

        public void GetBadges(StackExchangeApiCallback<List<Badge>> callback)
        {
            var path = String.Format("/me/badges?sort=name&order=asc&filter=!mxdR1yiG0J");
            GetStackExchangeObjects(path, callback);
        }

        private string ComposeUri(string path)
        {
            var uri = String.Format("{0}{1}", BaseUri, path);
            if (!String.IsNullOrWhiteSpace(_apiKey))
            {
                var separator = uri.Contains("?") ? "&" : "?";
                uri = String.Format("{0}{1}key={2}&site=stackoverflow&access_token={3}", uri, separator, _apiKey, _accessToken);
            }
            return uri;
        }

        private string ExtractJsonResponse(WebResponse response)
        {
            if (response == null)
                return null;

            string json;
            using (var outStream = new MemoryStream())
            using (var zipStream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress))
            {
                zipStream.CopyTo(outStream);
                outStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(outStream, Encoding.UTF8))
                {
                    json = reader.ReadToEnd();
                }
            }

            return json;
        }

        private void GetStackExchangeObject<T>(string path, StackExchangeApiCallback<T> callback) 
            where T : class, new()
        {
            var requestUri = ComposeUri(path);
            GetSingleObject(requestUri, callback);
        }

        private void GetSingleObject<T>(string requestUri, StackExchangeApiCallback<T> callback) 
            where T : class, new()
        {
            var request = (HttpWebRequest) WebRequest.Create(requestUri);
            request.Method = "GET";
            request.Accept = "application/json";

            var timer = new RequestTimeout(request, Timeout);

            request.BeginGetResponse(
                ar =>
                    {
                        try
                        {
                            var requestTimer = (RequestTimeout)ar.AsyncState;
                            requestTimer.Dispose();

                            var response = (HttpWebResponse) request.EndGetResponse(ar);
                            var json = ExtractJsonResponse(response);
                            var data = ParseJson<T>(json).FirstOrDefault();
                            callback(data);
                        }
                        catch (WebException ex)
                        {
                            var response = ex.Response as HttpWebResponse;

                            if (ex.Status == WebExceptionStatus.RequestCanceled || response == null)
                            {
                                var timeout = new Error { Id = (int)StackExchangeError.TemporarilyUnavailable };
                                FireError(timeout);
                                return;
                            }
                            
                            var json = ExtractJsonResponse(response);
                            var error = JsonConvert.DeserializeObject<Error>(json) ??
                                        new Error {Id = (int) StackExchangeError.TemporarilyUnavailable};

                            FireError(error);
                        }
                        catch (Exception ex)
                        {
                            var error = new Error { Id = (int)StackExchangeError.InternalError, Message = ex.Message, Name = ex.GetType().Name };
                            FireError(error);
                        }
                    },
                timer);
        }

        private void GetMultipleObjects<T>(string requestUri, StackExchangeApiCallback<List<T>> callback) where T : class, new()
        {
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.Method = "GET";
            request.Accept = "application/json";

            var timer = new RequestTimeout(request, Timeout);

            request.BeginGetResponse(
                ar =>
                {
                    try
                    {
                        var requestTimer = (RequestTimeout)ar.AsyncState;
                        requestTimer.Dispose();

                        var response = (HttpWebResponse) request.EndGetResponse(ar);
                        var json = ExtractJsonResponse(response);
                        var data = ParseJson<T>(json).ToList();
                        callback(data);
                    }
                    catch (WebException ex)
                    {
                        var response = ex.Response as HttpWebResponse;

                        if (ex.Status == WebExceptionStatus.RequestCanceled || response == null)
                        {
                            var timeout = new Error { Id = (int)StackExchangeError.TemporarilyUnavailable };
                            FireError(timeout);
                            return;
                        }

                        var json = ExtractJsonResponse(response);
                        var error = JsonConvert.DeserializeObject<Error>(json) ??
                                    new Error {Id = (int) StackExchangeError.TemporarilyUnavailable};

                        FireError(error);
                    }
                    catch (Exception ex)
                    {
                        var error = new Error { Id = (int)StackExchangeError.InternalError, Message = ex.Message, Name = ex.GetType().Name };
                        FireError(error);
                    }
                },
                timer);
        }

        private static IEnumerable<T> ParseJson<T>(string json) where T : class, new()
        {
            var type = typeof(T);
            var attribute = type.GetCustomAttributes(typeof(WrapperObjectAttribute), false).SingleOrDefault() as WrapperObjectAttribute;
            if (attribute == null)
            {
                throw new InvalidOperationException(
                    String.Format("{0} type must be decorated with a WrapperObjectAttribute.", type.Name));
            }

            var jobject = JObject.Parse(json);
            var collection = JsonConvert.DeserializeObject<List<T>>(jobject[attribute.WrapperObject].ToString());
            return collection;
        }

        private void GetStackExchangeObjects<T>(string path, StackExchangeApiCallback<List<T>> callback) where T : class, new()
        {
            var requestUri = ComposeUri(path);
            GetMultipleObjects(requestUri, callback);
        }

        private void FireError(Error error)
        {
            var handler = Error;
            if (handler != null)
            {
                handler(this, new StackExchangeErrorEventArgs(error));
            }
        }
    }
}
