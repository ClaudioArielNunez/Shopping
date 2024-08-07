using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
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

            //Agregamos restricciones de user, role
            builder.Services.AddIdentity<User, IdentityRole>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true; //email unico
                cfg.Password.RequireDigit = false; //restricciones de password
                cfg.Password.RequiredUniqueChars = 0;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
                //cfg.Password.RequiredLength = 6; //largo minimo por defecto es 6
            }).AddEntityFrameworkStores<DataContext>();

            //Agregamos control de cookies
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/NotAuthorized";
                options.AccessDeniedPath = "/Account/NotAuthorized";
            });

            //Agregamos SeedDb
            builder.Services.AddTransient<SeedDb>();
            //Agregamos userHelper
            builder.Services.AddScoped<IUserHelper,UserHelper>();

            var app = builder.Build();

            //creamos metodo para llamar al Seed
            SeedData(app);

            void SeedData(WebApplication app) 
            { 
                // Obtenemos el servicio IServiceScopeFactory del contenedor de servicios de la aplicación
                IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

                // Creamos un nuevo scope para limitar el tiempo de vida de los servicios dentro de él
                using (IServiceScope? scope = scopedFactory.CreateScope())
                {
                    // Obtenemos el servicio SeedDb del proveedor de servicios dentro del scope
                    SeedDb? service = scope.ServiceProvider.GetService<SeedDb>();

                    // Llamamos al método SeedAsync en SeedDb para sembrar los datos de forma asíncrona
                    service.SeedAsync().Wait();
                    //el .Wait() bloquea la ejecución hasta que el método asíncrono complete su
                    //ejecución, lo que es síncrono en términos de la ejecución actual del hilo.
                }
            }


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Configuramos cuando la pagina no se encuentre
            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //para los usuarios
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}