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

                image.Url = this.SaveLargeImage(photoBytes, file.FileName);
                image.Thumbnail = this.SaveThumbnail(photoBytes, file.FileName);

                result.Add(image);
            }

            return result;
        }

        private string SaveThumbnail(byte[] bytes, string fileName)
        {
            using (MemoryStream inStream = new MemoryStream(bytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (ImageFactory imageFactory = new ImageFactory())
                    {
                        // Load, resize, set the format and quality and save an image.
                        imageFactory.Load(inStream);
                        imageFactory.Resize(new Size(200, 0));
                        imageFactory.Format(ImageFormat.Jpeg);
                        imageFactory.Quality(80);
                        imageFactory.Save(outStream);
                    }

                    outStream.Position = 0;
                    var thumbnailResult = this._fileStorage.UploadFile(outStream, "thumb-" + fileName, "products/");
                    return thumbnailResult.Uri.ToString();
                }
            }
        }

        private string SaveLargeImage(byte[] bytes, string fileName)
        {
            using (MemoryStream inStream = new MemoryStream(bytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    using (ImageFactory imageFactory = new ImageFactory())
                    {
                        // Load, resize, set the format and quality and save an image.
                        imageFactory.Load(inStream);
                        int width = imageFactory.Image.Size.Width;
                        int height = imageFactory.Image.Size.Height;

                        if (width > 1600)
                        {
                            int newHeight = (int)((1600 / (double)width) * height);
                            imageFactory.Resize(new Size(1600, newHeight));
                            imageFactory.Format(ImageFormat.Jpeg);
                            imageFactory.Quality(80);
                            imageFactory.Save(outStream);
                        }
                        else if (height > 1600)
                        {
                            int newWidth = (int)((1600 / (double)height) * width);
                            imageFactory.Resize(new Size(newWidth, 1600));
                            imageFactory.Format(ImageFormat.Jpeg);
                            imageFactory.Quality(80);
                            imageFactory.Save(outStream);
                        }
                        else
                        {
                            imageFactory.Save(outStream);
                        }
                    }

                    outStream.Position = 0;
                    var resizedResult = this._fileStorage.UploadFile(outStream, fileName, "products/");
                    return resizedResult.Uri.ToString();
                }
            }
        }
    }
}
