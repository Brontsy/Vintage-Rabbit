using System.Net;
using System.IO;

namespace Vintage.Rabbit.Common.Http
{
    /// <summary>
    /// This wrapper class helps us mock out the http web response when make a http web request.
    /// </summary>
    public interface IHttpWebResponse<T>
    {
        T Response { get; }

        /// <summary>
        /// Gets the status code of our web response
        /// </summary>
        HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Returns a byte array containing the response body.
        /// </summary>
        byte[] ResponseBody { get; }

        /// <summary>
        /// Gets a specific http response header
        /// </summary>
        string GetHeader(string header);

        /// <summary>
        /// Returns a new MemoryStream containing reponse body. The caller is responsible for closing the stream.
        /// </summary>
        Stream GetResponseStream();

        string Body { get; }
    }
}
