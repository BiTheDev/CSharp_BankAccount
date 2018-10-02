using Microsoft.EntityFrameworkCore;
 
namespace BankAccounts.Models
{
    public class BankContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public BankContext(DbContextOptions<BankContext> options) : base(options) { }
        public DbSet<accounts> accounts{get;set;}

        public DbSet<Users> users{get;set;}
        
    }
}