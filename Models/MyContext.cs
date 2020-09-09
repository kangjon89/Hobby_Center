using Microsoft.EntityFrameworkCore;

namespace Exam.Models
{
    public class MyContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Sport> Sports { get; set; }
        public DbSet<Join> Joins { get; set; }

    }
}