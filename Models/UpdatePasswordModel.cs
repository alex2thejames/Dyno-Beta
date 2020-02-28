using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace Dyno.Models
{
    public class UpdatePassword
    {
    
    [DataType(DataType.Password)]
    [Required]
    public string OldPassword {get;set;}

    [DataType(DataType.Password)]
    [Required]
    public string Password {get;set;}

    // Will not be mapped to your users table!
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string Confirm {get;set;}
    }
}