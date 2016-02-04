using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Blog.Models
{
    [Table("ImageMetaData")]
    public class ImageMetaDataModel
    {
        [Key]
        public long? ListId { get; set; }
        public string ImageHeight { get; set; }
        public string ImageWidth { get; set; }
        public string CameraBrand { get; set; }
        public string CameraModel { get; set; }
        public string  Software { get; set; }
        public string HandleTime { get; set; }
        public string Exposure { get; set; }
        public string Aperture  { get; set; }
        public string  FocusProgram { get; set; }
        public string Iso { get; set; }
        public string CaptureTime { get; set; }
        public string Flash { get; set; }
        public string  ColorSpace { get; set; }
        public string  FocusLength { get; set; }
        public string ExposureMode { get; set; }
        public string  WhiteBalanceMode { get; set; }
        public string LensModel { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
    }
}