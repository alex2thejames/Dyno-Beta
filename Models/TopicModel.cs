using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace Dyno.Models
{
    public class Topic
    {
        [Key]
        public int TopicId {get;set;}

        [Required]
        [MinLength(2, ErrorMessage="Name must be 2 characters or longer!")]
        public string TopicName {get;set;}
        public string TopicLink {get;set;}

        public string TopicTitle {get;set;}
        public string TopicIMG {get;set;}

        public DateTime CreatedAt {get;set;} = DateTime.Now;
    
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public List<Interest> Interests {get;set;}
    }
}