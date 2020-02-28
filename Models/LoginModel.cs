using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Dyno.Models
{
    public class Login
    {
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email {get;set;}
    
    // [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password {get;set;}
    }
}