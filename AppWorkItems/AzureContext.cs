using AppWorkItems.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppWorkItems
{
    public class AzureContext : DbContext
    {
        public DbSet<Items> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=TesteEngenhoWorkItem;Trusted_Connection=True;");
        }
    }
}
