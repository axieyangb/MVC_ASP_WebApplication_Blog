using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogTest.Models
{
    [Table("Tags")]
    public class Tags
    {
        [Key]
        [Column("TagID")]

        public long? TagId { get; set; }
        [DataType(DataType.Text)]
        [Column("TagContent")]
        public string TagContent { get; set; }
        [Column("TagCount")]
        public int TagCount { get; set; }
        [Column("LastUsedDate")]
        public DateTime? LastUsedDate { get; set; }
    }
}