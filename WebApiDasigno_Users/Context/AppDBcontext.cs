using Microsoft.EntityFrameworkCore;
using WebApiDasigno_Users.Models;

namespace WebApiDasigno_Users.Context
{
    public class AppDBcontext:DbContext
    {
        public AppDBcontext(DbContextOptions<AppDBcontext> options)
            : base(options)
        {
            
        }

        public DbSet<Users> Users { get; set; }
    }
}
