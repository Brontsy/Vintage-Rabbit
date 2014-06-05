using ImageProcessor;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Vintage.Rabbit.Products.Entities;
using Vintage.Rabbit.Products.Result;

namespace Vintage.Rabbit.Products.Services
{
    public interface IUploadProductImageService
    {
        IList<ProductImage> UploadFiles(HttpFileCollectionBase files);
    }

    internal class UploadProductImageService : IUploadProductImageService
    {
        private IFileStorage _fileStorage;

        public UploadProductImageService(IFileStorage fileStorage)
        {
            this._fileStorage = fileStorage;
        }

        public IList<ProductImage> UploadFiles(HttpFileCollectionBase files)
        {
            IList<ProductImage> result = new List<ProductImage>();

            foreach (string key in files.Keys)
            {
                var file = files[key];

                byte[] photoBytes = new byte[file.ContentLength];
                file.InputStream.Read(photoBytes, 0, file.ContentLength);
                var thumbnailSize = new Size(200, 0);

                ProductImage image = new ProductImage();

                using (MemoryStream inStream = new MemoryStream(photoBytes))
                {
                    var uploadResult = this._fileStorage.UploadFile(inStream, file.FileName, "products/");
                    image.Url = uploadResult.Uri.ToString();

                    using (MemoryStream outStream = new MemoryStream())
                    {
                        using (ImageFactory imageFactory = new ImageFactory())
                        {
                            // Load, resize, set the format and quality and save an image.
                            imageFactory.Load(inStream);
                            imageFactory.Resize(thumbnailSize); 
                            imageFactory.Format(ImageFormat.Jpeg);
                            imageFactory.Quality(80);
                            imageFactory.Save(outStream);
                        }

                        outStream.Position = 0;
                        var thumbnailResult = this._fileStorage.UploadFile(outStream, "thumb-"+file.FileName, "products/");
                        image.Thumbnail = thumbnailResult.Uri.ToString();
                    }

                    result.Add(image);
                }
            }

            return result;
        }
    }
}
