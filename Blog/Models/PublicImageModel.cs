using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace Blog.Models
{
    [Table("VW_PublicImage")]
    public class PublicImageViewModel
    {
        [Key]
        [Column("PublicID")]
        public long? PublicId { get; set; }
        [Column("PictureID")]
        public long? PictureId { get; set; }
        [Column("UserID")]
        public long? UserId { get; set; }
        [Column("Url")]
        public string Url  { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("FileName")]
        public string FileName { get; set; }
        [Column("Rate")]
        public double? Rate { get; set; }
        [Column("Like")]
        public int Like { get; set; }
        [Column("AuthorName")]
        public string AuthorName {get;set;}
        [Column("ContentType")]
        public string ContentType { get; set; }
        [Column("ImageHeight")]
        public string ImageHeight { get; set; }
        [Column("ImageWidth")]
        public string ImageWidth { get; set; }
        [Column("CameraModel")]
        public string CameraModel { get; set; }
        [Column("Software")]
        public string Software { get; set; }
        [Column("Exposure")]
        public string Exposure { get; set; }
        [Column("Aperture")]
        public string Aperture { get; set; }
        [Column("FocusProgram")]
        public string FocusProgram { get; set; }
        [Column("ISO")]
        public string Iso { get; set; }
        [Column("CaptureTime")]
        public string CaptureTime { get; set; }
        [Column("Flash")]
        public string Flash { get; set; }
        [Column("FocusLength")]
        public string FocusLength { get; set; }
        [Column("WhiteBalanceMode")]
        public string WhiteBalanceMode { get; set; }
        [Column("LensModel")]
        public string LensModel { get; set; }
    }

    [Table("PublicImage")]
    public class PublicImageModel
    {
        [Key]
        [Column("PublicID")]
        public long? PublicId { get; set; }
        [Column("PictureID")]
        public long PictureId { get; set; }
        [Column("UserID")]
        public long UserId { get; set; }
        [Column("Description")]
        public string Description { get; set; }
        [Column("isBlock")]
        public byte IsBlock { get; set; }
        [Column("RecentBePublicDate")]
        public DateTime RecentBePublicDate { get; set; }
        [Column("RecentBePrivateDate")]
        public DateTime? RecentBePrivateDate { get; set; }
        [Column("Rate")]
        public double? Rate { get; set; }
        [Column("Like")]
        public int Like { get; set; }
    }
}