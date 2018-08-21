using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Query.AspNetCore.Example.Data;

namespace Spectre.Query.AspNetCore.Example
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<MovieContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Database")));

            // Configure the query provider.
            services.AddQueryProvider<MovieContext>(options =>
            {
                // Configure an entity.
                options.Configure<Movie>(movie =>
                {
                    movie.Map("Id", e => e.MovieId);
                    movie.Map("Genre", e => e.Genre.Name);
                    movie.Map("Title", e => e.Name);
                    movie.Map("Year", e => e.ReleasedAt);
                    movie.Map("Score", e => e.Rating);
                    movie.Map("Seen", e => e.Seen);
                });

                // Configure a query type projection.
                options.Configure<MovieProjection>(movie =>
                {
                    movie.Map("Id", e => e.MovieId);
                    movie.Map("Genre", e => e.Genre);
                    movie.Map("Title", e => e.Name);
                    movie.Map("Year", e => e.ReleasedAt);
                    movie.Map("Score", e => e.Rating);
                    movie.Map("Seen", e => e.Seen);
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // This is not required, but will make sure that all
            // initialization is performed at start up and not at
            // the first time the query provider is used.
            app.UseQueryProvider<MovieContext>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
