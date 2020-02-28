using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace Dyno.Models
{
    public class Interest
    {
        [Key]
        public int InterestId {get;set;}
        public int UserId {get;set;}
        public int TopicId {get;set;}
        public User User {get;set;}
        public Topic Topic {get;set;}
    }
}