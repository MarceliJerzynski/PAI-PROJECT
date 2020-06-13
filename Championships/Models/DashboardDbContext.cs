using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Championships.Models
{
    public class DashboardDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Tournament> Tournaments { get; set; }
    }
}