using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MetadataExtractor;
using System.Collections;
using System.Configuration;
using System.Data.Entity;
using Blog.Models;
namespace Blog.Controllers
{
    public class ImageMetaData
    {
        private readonly BlogContext _db= new BlogContext(ConfigurationManager.ConnectionStrings["BlogContext"].ConnectionString);
        private IEnumerable<Directory> _directories;
        private readonly ImageMetaDataModel _oneImage;

        public  ImageMetaData(string imageFilePath)
        {
            _directories = ImageMetadataReader.ReadMetadata(imageFilePath);
            _oneImage = new ImageMetaDataModel {Url = imageFilePath};
            string [] splits = imageFilePath.Split('\\');
            _oneImage.FileName = splits[splits.Length - 1];
        }
        public  ImageMetaData(long imageId)
        {
            var query = from image in _db.Images
                        where image.ImageId == imageId
                        select image;
            _directories =ImageMetadataReader.ReadMetadata(query.ElementAt(0).Url);
            _oneImage = new ImageMetaDataModel {Url = query.ElementAt(0).Url};
        }
        public void SetImagePath(string imageFilePath)
        {
            _directories = ImageMetadataReader.ReadMetadata(imageFilePath);
        }

        public void FetchData()
        {

                foreach (var directory in _directories)
                foreach (var tag in directory.Tags)
                {
                    if (directory.Name.Equals("JPEG"))
                    {
                        if (tag.TagName.Equals("Image Height"))
                            _oneImage.ImageHeight = tag.Description;
                        else if (tag.TagName.Equals("Image Width"))
                            _oneImage.ImageWidth = tag.Description;
                    }
                    else if (directory.Name.Equals("Exif IFD0"))
                    {
                        if (tag.TagName.Equals("Make"))
                            _oneImage.CameraBrand = tag.Description;
                        else if (tag.TagName.Equals("Model"))
                            _oneImage.CameraModel = tag.Description;
                        else if (tag.TagName.Equals("Software"))
                            _oneImage.Software = tag.Description;
                        else if (tag.TagName.Equals("Date/Time"))
                            _oneImage.HandleTime = tag.Description;
                    }
                    else if (directory.Name.Equals("Exif SubIFD"))
                    {
                        if (tag.TagName.Equals("Exposure Time"))
                            _oneImage.Exposure = tag.Description;
                        else if (tag.TagName.Equals("Date/Time"))
                            _oneImage.HandleTime = tag.Description;
                        else if (tag.TagName.Equals("Aperture Value"))
                            _oneImage.Aperture =tag.Description;
                        else if (tag.TagName.Equals("Exposure Program"))
                            _oneImage.FocusProgram=tag.Description;
                        else if (tag.TagName.Equals("ISO Speed Ratings"))
                            _oneImage.Iso=tag.Description;
                        else if (tag.TagName.Equals("Date/Time Original"))
                            _oneImage.CaptureTime = tag.Description;
                        else if (tag.TagName.Equals("Flash"))
                            _oneImage.Flash = tag.Description;
                        else if (tag.TagName.Equals("Color Space"))
                            _oneImage.ColorSpace = tag.Description;
                        else if (tag.TagName.Equals("Focal Length"))
                            _oneImage.FocusLength = tag.Description;
                        else if (tag.TagName.Equals("Exposure Mode"))
                            _oneImage.ExposureMode = tag.Description;
                        else if (tag.TagName.Equals("White Balance Mode"))
                            _oneImage.WhiteBalanceMode = tag.Description;
                        else if (tag.TagName.Equals("Lens Model"))
                            _oneImage.LensModel = tag.Description;
                    }
                }

        }
        public ImageMetaDataModel GetMetaData()
        {
            return _oneImage;
        }
    }
}