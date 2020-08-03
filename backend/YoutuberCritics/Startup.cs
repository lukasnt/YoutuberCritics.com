using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using YoutuberCritics.Data;
using YoutuberCritics.Services;
using YoutuberCritics.Services.Scrapers;

namespace YoutuberCritics
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddDbContext<YoutuberCriticsContext>(options => {
                options.UseSqlServer(Configuration.GetConnectionString("Azure"));
            });


            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000", "https://youtubercritics.com").AllowAnyHeader().AllowAnyMethod();
                    });
            });

            /*
            var options = new ChromeOptions();
            options.AddArguments("headless", "--lang=en");
            services.AddSingleton<RemoteWebDriver, ChromeDriver>(provider => new ChromeDriver(options));
            services.AddSingleton<RemoteWebDriver, ChromeDriver>(provider => new ChromeDriver(options));
            */
            
            services.AddSingleton<IScraper, HttpScraper>();

            services.AddTransient<SearchService>();
            services.AddSingleton<CacheService>();

            services.AddControllers();

            //services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            /*
            app.UseCors(options =>
                options.WithOrigins("http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod());
            */
            app.UseCors();
            

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
