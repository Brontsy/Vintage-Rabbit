using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vintage.Rabbit.Products.Result
{
    public class UploadProductImageResult
    {        /// <summary>
        /// Gets the id of this storage item
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets the url to access this storage item
        /// </summary>
        public Uri Uri { get; set; }

        /// <summary>
        /// Gets the name of the storage item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the host of this storage item www.castlerock.blobstorage.com.au etc
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Gets the path to this storage item /Images/Properties/Boronia etc
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets the scheme of the storage item http or https
        /// </summary>
        public string Scheme { get; set; }

        public UploadProductImageResult(Uri url)//string scheme, string host, string path, string name)
        {
            this.Uri = url;
            this.Scheme = url.Scheme;
            this.Host = url.Host + "/" + url.Segments[1];

            string localPath = url.LocalPath;

            if (localPath.EndsWith("/"))
            {
                localPath = localPath.Substring(0, localPath.Length - 1);
            }

            var segments = localPath.Split('/');

            this.Name = segments[segments.Length - 1];
            for (int i = 2; i < segments.Length - 1; i++) 
            { 
                this.Path += segments[i] + "/"; 
            }
        }
    }
}
