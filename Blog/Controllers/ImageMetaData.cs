using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MetadataExtractor;
using System.Collections;
using System.Data.Entity;
using Blog.Models;
namespace Blog.Controllers
{
    public class ImageMetaData
    {
        private IEnumerable<Directory> directories;
        private BlogContext db;
        private ImageMetaDataModel oneImage;

        public  ImageMetaData(string ImageFilePath)
        {
            directories = ImageMetadataReader.ReadMetadata(ImageFilePath);
            oneImage = new ImageMetaDataModel();
            oneImage.URL = ImageFilePath;
            string [] splits = ImageFilePath.Split('\\');
            oneImage.FileName = splits[splits.Length - 1];
        }
        public  ImageMetaData(long ImageID)
        {
            db = new BlogContext();
            var query = from image in db.Images
                        where image.ImageID == ImageID
                        select image;
            directories =ImageMetadataReader.ReadMetadata(query.ElementAt(0).Url.ToString());
            oneImage = new ImageMetaDataModel();
            oneImage.URL = query.ElementAt(0).Url.ToString();
        }
        public void SetImagePath(string ImageFilePath)
        {
            directories = ImageMetadataReader.ReadMetadata(ImageFilePath);
        }

        public void fetchData()
        {

                foreach (var directory in directories)
                foreach (var tag in directory.Tags)
                {
                    if (directory.Name.Equals("JPEG"))
                    {
                        if (tag.TagName.Equals("Image Height"))
                            oneImage.ImageHeight = tag.Description;
                        else if (tag.TagName.Equals("Image Width"))
                            oneImage.ImageWidth = tag.Description;
                    }
                    else if (directory.Name.Equals("Exif IFD0"))
                    {
                        if (tag.TagName.Equals("Make"))
                            oneImage.CameraBrand = tag.Description;
                        else if (tag.TagName.Equals("Model"))
                            oneImage.CameraModel = tag.Description;
                        else if (tag.TagName.Equals("Software"))
                            oneImage.Software = tag.Description;
                        else if (tag.TagName.Equals("Date/Time"))
                            oneImage.HandleTime = tag.Description;
                    }
                    else if (directory.Name.Equals("Exif SubIFD"))
                    {
                        if (tag.TagName.Equals("Exposure Time"))
                            oneImage.Exposure = tag.Description;
                        else if (tag.TagName.Equals("Date/Time"))
                            oneImage.HandleTime = tag.Description;
                        else if (tag.TagName.Equals("Aperture Value"))
                            oneImage.Aperture =tag.Description;
                        else if (tag.TagName.Equals("Exposure Program"))
                            oneImage.FocusProgram=tag.Description;
                        else if (tag.TagName.Equals("ISO Speed Ratings"))
                            oneImage.ISO=tag.Description;
                        else if (tag.TagName.Equals("Date/Time Original"))
                            oneImage.CaptureTime = tag.Description;
                        else if (tag.TagName.Equals("Flash"))
                            oneImage.Flash = tag.Description;
                        else if (tag.TagName.Equals("Color Space"))
                            oneImage.ColorSpace = tag.Description;
                        else if (tag.TagName.Equals("Focal Length"))
                            oneImage.FocusLength = tag.Description;
                        else if (tag.TagName.Equals("Exposure Mode"))
                            oneImage.ExposureMode = tag.Description;
                        else if (tag.TagName.Equals("White Balance Mode"))
                            oneImage.WhiteBalanceMode = tag.Description;
                        else if (tag.TagName.Equals("Lens Model"))
                            oneImage.LensModel = tag.Description;
                    }
                }

        }
        public ImageMetaDataModel getMetaData()
        {
            return oneImage;
        }
    }
}