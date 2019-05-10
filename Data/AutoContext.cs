using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Automation.API.Models
{
    public class AutoContext : DbContext
    {
        public AutoContext (DbContextOptions<AutoContext> options)
            : base(options)
        {
        }

        public DbSet<Automation.API.Models.Action> Action { get; set; }
        public DbSet<Condition> Condition { get; set; }
        public DbSet<Trigger> Trigger { get; set; }
        public DbSet<MetaData> MetaData { get; set; }
    }
}
