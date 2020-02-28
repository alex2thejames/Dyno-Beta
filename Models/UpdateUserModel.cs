using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace Dyno.Models
{
    public class UpdateUser
    {
    [MinLength(2)]
    public string FirstName {get;set;}
    
    [MinLength(2)]
    public string LastName {get;set;}
    
    [EmailAddress]
    public string Email {get;set;}

    
    [DataType(DataType.Password)]
    public string Password {get;set;}
    
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    // Will not be mapped to your users table!
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string Confirm {get;set;}
    }
}