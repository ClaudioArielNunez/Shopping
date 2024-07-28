﻿using Shopping.Data.Entities;

namespace Shopping.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context) //inyectamos el context db
        {
            _context = context;
        }

        //Este método se encarga de sembrar datos iniciales en tu base de datos utilizando
        //servicios registrados en tu contenedor de servicios (IServiceProvider). 
        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); //crea db y aplica migraciones
            await CheckCategoriesAsync(); //crea las categorias cuando no existen
            await CheckCountriesAsync();   //crea los paises cuando no existen
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Antioquía",
                            Cities = new List<City>()
                            {
                                new City { Name = "Medellín"},
                                new City { Name = "Envigado"},
                                new City { Name = "Itaguí"},
                                new City { Name = "Bello"},
                                new City { Name = "Rionegro"},
                            }
                        },
                        new State()
                        {
                            Name = "Bogotá",
                            Cities = new List<City>()
                            {
                                new City() { Name = "Usaquen" },
                                new City() { Name = "Champinero" },
                                new City() { Name = "Santa fe" },
                                new City() { Name = "Useme" },
                                new City() { Name = "Bosa" },
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name = "Florida",
                            Cities = new List<City>()
                            {
                                new City() { Name = "Orlando" },
                                new City() { Name = "Miami" },
                                new City() { Name = "Tampa" },
                                new City() { Name = "Fort Lauderdale" },
                                new City() { Name = "Key West" },
                            }
                        },
                        new State()
                        {
                            Name = "Texas",
                            Cities = new List<City>()
                            {
                                new City() { Name = "Houston" },
                                new City() { Name = "San Antonio" },
                                new City() { Name = "Dallas" },
                                new City() { Name = "Austin" },
                                new City() { Name = "El Paso" },
                            }
                        },
                    }
                });
            }
            await _context.SaveChangesAsync();
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Tecnología" });
                _context.Categories.Add(new Category { Name = "Ropa" });
                _context.Categories.Add(new Category { Name = "Calzado" });
                _context.Categories.Add(new Category { Name = "Belleza" });
                _context.Categories.Add(new Category { Name = "Nutrición" });
                _context.Categories.Add(new Category { Name = "Deportes" });
                _context.Categories.Add(new Category { Name = "Apple" });
                _context.Categories.Add(new Category { Name = "Mascotas" });

                await _context.SaveChangesAsync();//guardamos
            }
        }
    } //Paso siguente: inyectamos el servicio en el program
}
