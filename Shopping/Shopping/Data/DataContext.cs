using Microsoft.EntityFrameworkCore;
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
            //indico q creen  indices sobre el campo name para que no se repitan nombres
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();

            //puede haber nombre de estados repetidos, pero solo 1 por Pais
            modelBuilder.Entity<State>().HasIndex("Name","CountryId").IsUnique();
            //puede haber nombre de ciudades repetidos, pero solo 1 por Estado
            modelBuilder.Entity<City>().HasIndex("Name", "StateId").IsUnique();
        }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<State> States { get; set; }
    }
}
