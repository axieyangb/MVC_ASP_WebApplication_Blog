﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
namespace Blog.Models
{
     [Table("Member")]
    public class Member
    {
         [Key]
         public long? UserId { set; get; }
         [Required(ErrorMessage="Please provide username",AllowEmptyStrings=false)]
         public string UserName { set; get; }
         [Required(ErrorMessage="Please provide password",AllowEmptyStrings=false)]
         [DataType(DataType.Password)]
         public string Password { set; get; }
         [DataType(DataType.MultilineText)]
         public string Description { set; get; }
          [DataType(DataType.EmailAddress)]
         public string Email { set; get; }
         public string NickName { set; get; }
         public byte IsActive { set; get; }
    }
}