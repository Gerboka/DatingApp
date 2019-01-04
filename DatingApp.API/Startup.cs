using DatingApp.API.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DatingApp.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {   
            /*A Program indulásakor a Configuration az appsetting.Development.json vagy
            az appsettings.json fájlokból attól függ milyen módban vagyunk átemeli az ott elmentett
            beállításokat, konfigurációkat. */
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {   /* Indulásnál alap szolgáltatásként el kell indulni a DataContext osztálynak az 
            adatkapcsolat megléte miatt, és megadjuk az sql típusát Sqlite és a connection string-et is megadjuk
            paraméterként. A DataContext láthatóság miatt referenciába át kellet adni using DatingApp.API.Models; ezt.
            Az Sqlite hoz pedig Nuget managerrel hozzá kellet adni a Microsoft.EntityFrameworkCore.Sqlite-ot ennek a menete 
            a Session 2 ben a 8. videóban található*/
            /* A ConnectionStringet a Configuration vagyis az appsetting.jason vagy appsettings.Development.json 
            configurációs fájlokban megadott ConnectionString beállítás DefaulConnection-éből kinyertük a stringet */
            services.AddDbContext<DataContext>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //Ezt a sor a .NET API és az Aungular közötti megosztás miatt írtuk be
            services.AddCors();
            //Service ként hozzá adjok a programhoz az AuthRepositori interfész ést az AuthReposutory osztály
            services.AddScoped<IAuthRepository, AuthRepository>();
            //Ez az authentication-hoz kell, hogy tudjuk a controllerekben haasnálni ki melyik aprancsot hívhatja meg
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>{
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateIssuer = false,
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseHsts();
            }

            // app.UseHttpsRedirection();
            //Ezzal a sorral engedélyezzük az Origin,Header, És minden Method parancs
            // hozzáférését más programból jelen esetben az Angularból
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()); 
            //Ezt az induláshoz hozzá kel adni, hogy működjön a zauthentikáció így működni 
            //fog a Controllerekben a hozzá férés szabályozás
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
