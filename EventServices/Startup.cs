using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EventDataAccessLayer;
using EventDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EventServices
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
            Logger = LoggerFactory.Create(builder =>
            {
                builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("Startup", LogLevel.Debug)
                .AddConsole();
            }).CreateLogger<Startup>();
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; }
        public ILogger<Startup> Logger { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAutoMapper(typeof(Startup));

            services.AddDbContext<EventDBContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("EventDBConnectionString")));

            services.AddScoped<IEventRepository, EventRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEventRepository eventRepository)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            PopulateDatabase(eventRepository).GetAwaiter().GetResult();
        }

        private async Task PopulateDatabase(IEventRepository eventRepository)
        {
            eventRepository.ClearAllEvents();
            using (var httpClient = new HttpClient())
            {
                var apiUrl = Configuration["ApiSettings:Url"];
                var apiToken = Configuration["ApiSettings:Token"];

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
                var response = await httpClient.GetAsync(apiUrl);
                Logger.LogInformation($"API Response: {response}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStringAsync();
                    var events = JsonConvert.DeserializeObject<List<Events>>(apiResponse);

                    foreach (var eve in events)
                    {
                        eventRepository.AddEvent(eve);
                    }
                }
            }
        }
    }
}