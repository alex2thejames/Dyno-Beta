using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System;
namespace Dyno.Models
{
    public class Connection
    {
        [Key]
        public int ConnectionId {get;set;}

        public int FriendaId {get;set;}
        public User Frienda {get;set;}

        public int FriendbId {get;set;}
        public User Friendb {get;set;}

    }
}