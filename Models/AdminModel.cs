using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace Dyno.Models
{
    public class Admin
    {
        [Key]
        public int AdminId {get;set;}

        public bool WebSet {get;set;}

    }
}