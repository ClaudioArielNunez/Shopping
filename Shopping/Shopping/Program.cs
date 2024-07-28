using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Helpers;

namespace Shopping
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var con = builder.Configuration.GetConnectionString("DefaultConnection");

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(con));

            //Agregamos SeedDb
            builder.Services.AddTransient<SeedDb>();
            //Agregamos userHelper
            builder.Services.AddScoped<IUserHelper,UserHelper>();

            var app = builder.Build();

            //creamos metodo para llamar al Seed
            SeedData(app);

            void SeedData(WebApplication app) 
            { 
                // Obtenemos el servicio IServiceScopeFactory del contenedor de servicios de la aplicaci�n
                IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

                // Creamos un nuevo scope para limitar el tiempo de vida de los servicios dentro de �l
                using (IServiceScope? scope = scopedFactory.CreateScope())
                {
                    // Obtenemos el servicio SeedDb del proveedor de servicios dentro del scope
                    SeedDb? service = scope.ServiceProvider.GetService<SeedDb>();

                    // Llamamos al m�todo SeedAsync en SeedDb para sembrar los datos de forma as�ncrona
                    service.SeedAsync().Wait();
                    //el .Wait() bloquea la ejecuci�n hasta que el m�todo as�ncrono complete su
                    //ejecuci�n, lo que es s�ncrono en t�rminos de la ejecuci�n actual del hilo.
                }
            }




            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}