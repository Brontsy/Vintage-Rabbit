using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Vintage.Rabbit.Common.Http
{

    /// <summary>
    /// Helper class with useful HTTP Web Request/Response, etc related methods
    /// </summary>
    public class HttpWebUtility : IHttpWebUtility
    {

        public HttpWebUtility()
        {
        }

        public IHttpWebResponse<T> Get<T>(string url, int timeout, int maxAttempts = 1)
        {
            return this.Get<T>(url, timeout, null, null, new Dictionary<HttpRequestHeader, string>(), maxAttempts);
        }

        public IHttpWebResponse<T> Get<T>(string url, int timeout, ContentType type, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1)
        {
            return this.Get<T>(url, timeout, type, null, headers, maxAttempts);
        }

        public IHttpWebResponse<T> Get<T>(string url, int timeout, AcceptType accept, int maxAttempts = 1)
        {
            return this.Get<T>(url, timeout, null, accept, new Dictionary<HttpRequestHeader, string>(), maxAttempts);
        }

        public IHttpWebResponse<T> Get<T>(string url, int timeout, ContentType type, int maxAttempts = 1)
        {
            return this.Get<T>(url, timeout, type, null, new Dictionary<HttpRequestHeader, string>(), maxAttempts);
        }

        public IHttpWebResponse<T> Get<T>(string url, int timeout, ContentType contentType, AcceptType accept, int maxAttempts = 1)
        {
            return this.Get<T>(url, timeout, contentType, accept, new Dictionary<HttpRequestHeader, string>(), maxAttempts);
        }

        public IHttpWebResponse<T> Get<T>(string url, int timeout, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1)
        {
            return this.Get<T>(url, timeout, null, null, headers, maxAttempts);
        }

        public IHttpWebResponse<T> Get<T>(string url, int timeout, AcceptType acceptType, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1)
        {
            return this.Get<T>(url, timeout, null, acceptType, headers, maxAttempts);
        }

        private IHttpWebResponse<T> Get<T>(string url, int timeout, ContentType? contentType, AcceptType? accept, Dictionary<HttpRequestHeader, string> headers, int maxAttempts)
        {
            var request = ConstructRequest(url, "GET", contentType, accept, headers, timeout);
            return this.GetResponse<T>(request, null, maxAttempts);
        }

        public IHttpWebResponse<T> Post<T>(string url, int timeout, ContentType contentType, string data, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1)
        {
            var request = this.ConstructRequest(url, "POST", contentType, null, headers, timeout);
            var bytes = new UTF8Encoding().GetBytes(data);

            return this.GetResponse<T>(request, bytes, maxAttempts);
        }

        public IHttpWebResponse<T> Post<T>(string url, int timeout, ContentType contentType, byte[] bytes, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1)
        {
            var request = this.ConstructRequest(url, "POST", contentType, null, headers, timeout);

            return this.GetResponse<T>(request, bytes, maxAttempts);
        }

        public IHttpWebResponse<T> Put<T>(string url, int timeout, ContentType contentType, string data, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1)
        {
            var request = this.ConstructRequest(url, "PUT", contentType, null, headers, timeout);
            var bytes = new UTF8Encoding().GetBytes(data);

            return this.GetResponse<T>(request, bytes, 1);
        }

        public IHttpWebResponse<T> Delete<T>(string url, int timeout, ContentType contentType, Dictionary<HttpRequestHeader, string> headers, int maxAttempts = 1)
        {
            var request = ConstructRequest(url, "DELETE", contentType, null, headers, timeout);
            return this.GetResponse<T>(request, null, maxAttempts);
        }

        private IHttpWebResponse<T> GetResponse<T>(HttpWebRequest request, byte[] requestData, int maxAttempts)
        {
            var responseWrapper = new HttpWebResponseWrapper<T>(HttpStatusCode.BadRequest);
            var attempts = 0;
            var requestComplete = false;

            while (attempts < maxAttempts && !requestComplete)
            {
                attempts++;
                try
                {
                    if (requestData != null)
                    {
                        request.ContentLength = requestData.Length;
                        using (var requestStream = request.GetRequestStream())
                        {
                            requestStream.Write(requestData, 0, requestData.Length);
                        }
                    }

                    var response = (HttpWebResponse)request.GetResponse();

                    responseWrapper = new HttpWebResponseWrapper<T>(response);
                    requestComplete = true;
                }
                catch (WebException exception)
                {
                    var response = (HttpWebResponse)exception.Response;
                    if (exception.Status == WebExceptionStatus.Timeout)
                    {
                        responseWrapper = new HttpWebResponseWrapper<T>(HttpStatusCode.RequestTimeout);
                    }
                    else if (response != null)
                    {
                        responseWrapper = new HttpWebResponseWrapper<T>(response);
                        requestComplete = true;
                    }
                    else
                    {
                        // unexpected exception status
                        //_logger.Error(exception, LogTagHelper.GenerateStateTag(new { request, exception }), exception.Message);
                        requestComplete = true;
                    }

                    if (attempts < maxAttempts && responseWrapper.StatusCode == HttpStatusCode.RequestTimeout)
                    {
                        //_logger.Warn(string.Format("Url request sent to: {0}, Retry made: {1}", (request != null ? request.RequestUri.ToString() : string.Empty), attempts));
                    }
                    if (attempts == maxAttempts && responseWrapper.StatusCode == HttpStatusCode.RequestTimeout)
                    {
                        //_logger.Warn(string.Format("Timeout: Url request sent to: {0}, maximum tries made: {1}", (request != null ? request.RequestUri.ToString() : string.Empty), maxAttempts));
                    }
                }
            }

            return responseWrapper;
        }

        private HttpWebRequest ConstructRequest(string url, string httpVerb, ContentType? contentType, AcceptType? accept, Dictionary<HttpRequestHeader, string> headers, int requestTimeout
            )
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = httpVerb;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            if (contentType.HasValue)
            {
                switch (contentType)
                {
                    case ContentType.Json:
                        request.ContentType = "application/json";
                        break;

                    case ContentType.Xml:
                        request.ContentType = "text/xml";
                        break;

                    case ContentType.FormUrlEncoded:
                        request.ContentType = "application/x-www-form-urlencoded";
                        break;

                    case ContentType.Jpeg:
                        request.ContentType = "image/jpeg";
                        break;
                }
            }


            if (accept.HasValue)
            {
                switch (accept)
                {
                    case AcceptType.Json:
                        request.Accept = "application/json";
                        break;

                    case AcceptType.Xml:
                        request.Accept = "text/xml";
                        break;

                    case AcceptType.Stream:
                        request.Accept = "application/octet-stream";
                        break;
                }
            }

            request.Timeout = requestTimeout;

            if (headers != null)
            {
                foreach (var key in headers.Keys)
                {
                    request.Headers.Add(key, headers[key]);
                }
            }

            //AddTrackingHeaders(request);

            return request;
        }
    }
}
