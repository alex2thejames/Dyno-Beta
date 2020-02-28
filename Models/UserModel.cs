using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace Dyno.Models
{
    public class User
    {
    [Key]
    public int UserId {get;set;}

    [Required]
    [MinLength(2)]
    public string FirstName {get;set;}
    
    [Required]
    [MinLength(2)]
    public string LastName {get;set;}
    
    [Required]
    [EmailAddress]
    public string Email {get;set;}

    
    [DataType(DataType.Password)]
    [Required]
    public string Password {get;set;}

    [InverseProperty("Friendb")]
    public List<Connection> Friendas {get;set;}
    

    [InverseProperty("Frienda")]
    public List<Connection> Friendbs {get;set;}

    public List<Interest> Interests {get;set;}

    public bool Admin {get;set;}

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
    // Will not be mapped to your users table!
    [NotMapped]
    [Compare("Password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    public string Confirm {get;set;}
    }
}