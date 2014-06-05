using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.WindowsAzure;
using System.IO;
using System.Reflection;
using Vintage.Rabbit.Products.Result;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace Vintage.Rabbit.Products.Services
{
    internal interface IFileStorage
    {
        IList<UploadProductImageResult> UploadFiles(HttpFileCollectionBase files, string path);

        UploadProductImageResult UploadFile(Stream file, string fileName, string path);

        bool DeleteFile(string path);
    }

    internal class AzureBlobStorage : IFileStorage
    {
        public bool DeleteFile(string path)
        {
            if(path.Contains("http://vintagerabbit.blob.core.windows.net/vintage-rabbit/"))
            {
                path = path.Replace("http://vintagerabbit.blob.core.windows.net/vintage-rabbit/", string.Empty);
            }

            // Retrieve a reference to a container 
            CloudBlobContainer blobContainer = this.GetCloudBlobContainer();

            CloudBlockBlob cloudBlob = blobContainer.GetBlockBlobReference(path);

            // If we cannot delete a direct reference (ie a file) then we must be deleting a directory
            if (!cloudBlob.DeleteIfExists())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Upload a collection of files to the server
        /// </summary>
        /// <param name="files">the files to upload</param>
        /// <param name="website">the website these files are for</param>
        /// <param name="path">the path to the location to upload the files to</param>
        /// <returns>a colletion of result objects</returns>
        public IList<UploadProductImageResult> UploadFiles(HttpFileCollectionBase files, string path)
        {
            IList<UploadProductImageResult> result = new List<UploadProductImageResult>();

            foreach (string key in files.Keys)
            {
                result.Add(this.UploadFile(files[key], path));
            }

            return result;
        }

        /// <summary>
        /// Uploads a file to the image server
        /// </summary>
        /// <param name="file">the file to upload</param>
        /// <param name="domain">the domain this website lives in. eg castlerockproperty, auslinkproperty etc</param>
        /// <param name="path">The path to save the image to</param>
        /// <returns>result object with filename and success / fail messages</returns>
        public UploadProductImageResult UploadFile(HttpPostedFileBase file, string path)
        {
            return this.UploadFile(file.InputStream, file.FileName, path);
        }

        /// <summary>
        /// Uploads a file to the image server
        /// </summary>
        /// <param name="file">the file to upload</param>
        /// <param name="fileName">the name of the file to upload</param>
        /// <param name="domain">the domain this website lives in. eg castlerockproperty, auslinkproperty etc</param>
        /// <param name="path">The path to save the image to</param>
        /// <returns>result object with filename and success / fail messages</returns>
        public UploadProductImageResult UploadFile(Stream file, string fileName, string path)
        {
            CloudBlobContainer blobContainer = this.GetCloudBlobContainer();

            if (!string.IsNullOrEmpty(path) && !path.EndsWith("/"))
            {
                path += "/";
            }

            //ICloudBlob blob = blobContainer.GetBlobReferenceFromServer(path + fileName);

            //blob.Properties.ContentType = this.GetContentType(fileName);
            //// Create or overwrite the "myblob" blob with contents from a local file
            //blob.UploadFromStream(file);

            CloudBlockBlob blob = blobContainer.GetBlockBlobReference(path + fileName);
            blob.Properties.ContentType = this.GetContentType(fileName);
            blob.UploadFromStream(file);

            UploadProductImageResult uploadFile = new UploadProductImageResult(blob.Uri);

            return uploadFile;
        }

        private string GetContentType(string fileName)
        {
            if (fileName.Contains('.'))
            {
                string[] segments = fileName.Split('.');
                switch (segments[segments.Length - 1])
                {
                    case "pdf":
                        return "application/pdf";
                    case "jpg":
                    case "jpeg":
                        return "image/jpg";
                    case "png":
                        return "image/png";
                    case "gif":
                        return "image/gif";
                }
            }

            return "application/octet-stream";
        }

        private CloudBlobContainer GetCloudBlobContainer()
        {

            string connectionInfo = ConfigurationManager.AppSettings["BlobStorageConnectionString"];

            // Retrieve storage account from connection-string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionInfo);

            // Create the blob client 
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container 
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("vintage-rabbit");

            // Create the container if it doesn't already exist
            if (blobContainer.CreateIfNotExists())
            {
                blobContainer.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            return blobContainer;
        }
    }
}
