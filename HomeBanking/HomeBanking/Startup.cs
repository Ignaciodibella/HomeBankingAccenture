using HomeBanking.Models;
using HomeBanking.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace HomeBanking
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) //Acá vamos a injectar los servicios y controladores que creemos.
        {
            services.AddRazorPages();
            //?
            services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

            //Agregado del contexto de la BD:
            services.AddDbContext<HomeBankingContext>(options => options.UseSqlServer(Configuration.GetConnectionString("HomeBankingConexion")));
            
            //Agregado del Scoped.Instancia del servicio ClientRepository
            services.AddScoped<IClientRepository, ClientRepository>();

            //Agregado del Scoped.Instancia del servicio AccountRepository
            services.AddScoped<IAccountRepository, AccountRepository>();

            //Agregado del Scoped.Instancia del servicio CardRepository
            services.AddScoped<ICardRepository, CardRepository>();

            //Agregado del Scoped.Instancia del servicio TransactionRepository
            services.AddScoped<ITransactionRepository, TransactionRepository>();


            //Autenticación
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => { 
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.LoginPath = new PathString("/index.html");
                });

            //Autorización
            services.AddAuthorization
                (options =>
                {
                    options.AddPolicy("ClientOnly", policy => policy.RequireClaim("Client"));
                });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages(); //Razor: tecnología para crear paginas web con C#, HTML y CSS.       
                endpoints.MapControllers(); //Agrrega a los endpoints las clases que extienden de controllers.
                /*
                endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=games}/{ action = Get}");*/
                
            });
        }
    }
}
