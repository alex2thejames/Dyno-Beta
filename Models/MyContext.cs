using Microsoft.EntityFrameworkCore;

namespace Dyno.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options) { }
        public DbSet<User> Users {get;set;}
        public DbSet<Topic> Topics {get;set;}
        public DbSet<Interest> Interests {get;set;}
        public DbSet<Admin> Admins {get;set;}
        public DbSet<Connection> Connections {get;set;}
    }
}