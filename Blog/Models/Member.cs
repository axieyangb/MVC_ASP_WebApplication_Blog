using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Blog.Models
{
     [Table("Member")]
    public class Member
    {
         [Key]
         public long? UserID { set; get; }
         [Required(ErrorMessage="Please provide username",AllowEmptyStrings=false)]
         public string UserName { set; get; }
         [Required(ErrorMessage="Please provide password",AllowEmptyStrings=false)]
         [DataType(System.ComponentModel.DataAnnotations.DataType.Password)]
         public string Password { set; get; }
         [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
         public string Description { set; get; }
          [DataType(System.ComponentModel.DataAnnotations.DataType.EmailAddress)]
         public string Email { set; get; }
         public string NickName { set; get; }
         public byte isActive { set; get; }
    }
}