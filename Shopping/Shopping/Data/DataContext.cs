﻿using Microsoft.EntityFrameworkCore;
using Shopping.Data.Entities;

namespace Shopping.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //indico q cree un indice sobre el campo name para que no se repitan nombres
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
        }
        public DbSet<Country> Countries { get; set; }
    }
}
