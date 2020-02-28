using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace Dyno.Models
{
    public class TopicTemp
    {
        [Key]
        public int TopicId {get;set;}

        [Required]
        [MinLength(2, ErrorMessage="Topic Name must be 2 characters or longer!")]
        public string TopicName1 {get;set;}

        [Required]
        [MinLength(2, ErrorMessage="Topic Name must be 2 characters or longer!")]
        public string TopicName2 {get;set;}

        [Required]
        [MinLength(2, ErrorMessage="Topic Name must be 2 characters or longer!")]
        public string TopicName3 {get;set;}
        
        public User CreatedBy {get;set;}

        public List<Interest> Interests {get;set;}
    }
}