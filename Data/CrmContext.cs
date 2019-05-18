using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Automation.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace Crm.Context
{
    public class CrmContext : DbContext
    {
        public string Connection { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Campaign> Campaign { get; set; }
        public CrmContext()
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Connection);    
        }
        
    }

}