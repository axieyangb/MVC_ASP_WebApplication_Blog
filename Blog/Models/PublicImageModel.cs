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
        public long? PublicID { get; set; }
        public long? PictureID { get; set; }
        public long? UserID { get; set; }
        public string Url  { get; set; }
        public string Desacription { get; set; }
        public float? Rate { get; set; }
        public int Like { get; set; }
        public string AuthorName {get;set;}
        public string ContentType { get; set; }
    }

    [Table("PublicImage")]
    public class PublicImageModel
    {
        [Key]
        public long? PublicID { get; set; }
        public long PictureID { get; set; }
        public long UserID { get; set; }
        public string Description { get; set; }
        public byte isBlock { get; set; }
        public DateTime RecentBePublicDate { get; set; }
        public DateTime? RecentBePrivateDate { get; set; }
        public float? Rate { get; set; }
        public int Like { get; set; }
    }
}