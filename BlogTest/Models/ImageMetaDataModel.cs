using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogTest.Models
{
    [Table("ImageMetaData")]
    public class ImageMetaDataModel
    {
        [Key]
        [Column("ListID")]
        public long? ListId { get; set; }
        [Column("ImageHeight")]
        public string ImageHeight { get; set; }
        [Column("ImageWidth")]
        public string ImageWidth { get; set; }
        [Column("CameraBrand")]
        public string CameraBrand { get; set; }
        [Column("CameraModel")]
        public string CameraModel { get; set; }
        [Column("Software")]
        public string  Software { get; set; }
        [Column("HandleTime")]
        public string HandleTime { get; set; }
        [Column("Exposure")]
        public string Exposure { get; set; }
        [Column("Aperture")]
        public string Aperture  { get; set; }
        [Column("FocusProgram")]
        public string  FocusProgram { get; set; }
        [Column("ISO")]
        public string Iso { get; set; }
        [Column("CaptureTime")]
        public string CaptureTime { get; set; }
        [Column("Flash")]
        public string Flash { get; set; }
        [Column("ColorSpace")]
        public string  ColorSpace { get; set; }
        [Column("FocusLength")]
        public string  FocusLength { get; set; }
        [Column("ExposureMode")]
        public string ExposureMode { get; set; }
        [Column("WhiteBalanceMode")]
        public string  WhiteBalanceMode { get; set; }
        [Column("LensModel")]
        public string LensModel { get; set; }
        [Column("URL")]
        public string Url { get; set; }
        [Column("FileName")]
        public string FileName { get; set; }
    }
}