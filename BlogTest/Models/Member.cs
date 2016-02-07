using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogTest.Models
{
     [Table("Member")]
    public class Member
    {
         [Key]
        [Column("UserID")]
        public long? UserId { set; get; }
         [Required(ErrorMessage="Please provide username",AllowEmptyStrings=false)]
        [Column("UserName")]
        public string UserName { set; get; }
         [Required(ErrorMessage="Please provide password",AllowEmptyStrings=false)]
         [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
        [Column("Password")]
        public string Password { set; get; }
         [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        [Column("Description")]
        public string Description { set; get; }
          [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
        [Column("Email")]
        public string Email { set; get; }
        [Column("NickName")]
        public string NickName { set; get; }
        [Column("isActive")]
        public byte IsActive { set; get; }
    }
}