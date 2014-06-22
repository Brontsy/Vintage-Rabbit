using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace Vintage.Rabbit.Common.Http
{
    /// <summary>
    /// This wrapper class helps us mock out the http web response when make a http web request.
    /// </summary>
    public class HttpWebResponseWrapper<T> : IHttpWebResponse<T>
    {
        private readonly HttpStatusCode _httpStatusCode;
        private readonly long _contentLength;
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();

        /// <summary>
        /// Constructor to return the actual http web response
        /// </summary>
        /// <param name="response"></param>
        public HttpWebResponseWrapper(HttpWebResponse response)
        {
            if (response == null)
                return;

            _httpStatusCode = response.StatusCode;
            _contentLength = response.ContentLength;

            // set the response body
            using (var responseStream = response.GetResponseStream())
            using (var memoryStream = new MemoryStream())
            {
                responseStream.CopyTo(memoryStream);
                this.ResponseBody = memoryStream.ToArray();
                memoryStream.Position = 0;
                using (var sr = new StreamReader(memoryStream))
                {
                    Body = sr.ReadToEnd();
                }
            }

            // get all the http response headers
            foreach (string key in response.Headers.Keys)
            {
                this._headers.Add(key, response.Headers[key]);
            }
        }

        public HttpWebResponseWrapper(HttpStatusCode statusCode)
        {
            this._httpStatusCode = statusCode;
        }

        /// <summary>
        /// Returns a byte array containing the response body.
        /// </summary>
        public byte[] ResponseBody { get; private set; }


        /// <summary>
        /// Gets the status code of our web response
        /// </summary>
        public HttpStatusCode StatusCode
        {
            get { return _httpStatusCode; }
        }

        public long ContentLength
        {
            get { return _contentLength; }
        }

        public string Body { get; private set; }

        /// <summary>
        /// Gets a specific http response header
        /// </summary>
        public string GetHeader(string header)
        {
            if (_headers.ContainsKey(header))
            {
                return _headers[header];
            }

            return null;
        }

        /// <summary>
        /// Returns a new MemoryStream containing reponse body. The caller is responsible for closing the stream.
        /// </summary>
        public Stream GetResponseStream()
        {
            return new MemoryStream(this.ResponseBody);
        }

        public T Response
        {
            get
            {
                if (this.StatusCode == HttpStatusCode.OK)
                {
                    object o = null;
                    try
                    {
                        o = JsonConvert.DeserializeObject<T>(this.Body);
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.ToString());
                    }
                    return (T)o;
                }

                return default(T);
            }
        }
    }
}
