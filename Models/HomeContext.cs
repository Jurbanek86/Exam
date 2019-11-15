using Microsoft.EntityFrameworkCore;

namespace Exam.Models
{
    public class HomeContext : DbContext
    {
        public HomeContext(DbContextOptions options) : base(options){}

        public DbSet<User> Users {get;set;}
        public DbSet<Job> Jobs {get;set;}
        public DbSet<RSVP> RSVPs {get;set;}
    }
}