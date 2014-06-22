using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;

namespace Vintage.Rabbit.Common.Http
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHttpWebUtility
    {
        IHttpWebResponse<T> Get<T>(string url, int timeout, int maxAttempts = 1);

        IHttpWebResponse<T> Get<T>(string url, int timeout, AcceptType accept, int maxAttempts = 1);

        IHttpWebResponse<T> Get<T>(string url, int timeout, ContentType contentType, int maxAttempts = 1);

        IHttpWebResponse<T> Get<T>(string url, int timeout, ContentType contentType, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1);

        IHttpWebResponse<T> Get<T>(string url, int timeout, ContentType contentType, AcceptType accept, int maxAttempts = 1);

        IHttpWebResponse<T> Get<T>(string url, int timeout, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1);

        IHttpWebResponse<T> Get<T>(string url, int timeout, AcceptType acceptType, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1);

        IHttpWebResponse<T> Post<T>(string url, int timeout, ContentType contentType, string data, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1);

        IHttpWebResponse<T> Post<T>(string url, int timeout, ContentType contentType, byte[] data, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1);

        IHttpWebResponse<T> Put<T>(string url, int timeout, ContentType contentType, string body, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1);

        IHttpWebResponse<T> Delete<T>(string url, int timeout, ContentType contentType, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1);
    }
}
