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
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Campaign> Campaign { get; set; }

        public CrmContext(DbContextOptions options) : base(options)
        {}
        // protected override void OnConfiguring(DbContextOptionsBuilder options)
        // {
        //     options.UseSqlServer(@"Server=khang-pc\\sqlexpress;Database=DataContext;Trusted_connection=True;");    
        // }
    }

}