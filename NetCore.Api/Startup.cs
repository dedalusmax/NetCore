using NetCore.Api.Config;
using NetCore.Business.Services;
using NetCore.Data.Entities;
using NetCore.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using NetCore.Business.Authentication;
using AutoMapper;

namespace NetCore.Api
{
    public class Startup
    {
        private bool IsDevelopment { get; set; }
        private bool IsTest { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            // Configure authentication
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = TokenHelper.GetTokenValidationParameters(Configuration.GetSection("TokenProviderOptions"));
            });

            // Configure infrastructure
            services.AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            services.AddAutoMapper();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(CurrentUserInfoFactory.Instance);

            // Register token provider
            services.AddSingleton<ITokenProvider>(new TokenProvider(Configuration.GetSection("TokenProviderOptions")));

            services.AddSingleton(new Business.Models.AppOptions(Configuration.GetSection("AppOptions")));

            ConfigureDataRepositories(services);
            ConfigureBusinessServices(services);

            // Configure swagger
            services.AddSwaggerGen(options =>
            {
                // options.OperationFilter<AddAuthorizationHeaderParameterFilter>();

                options.SwaggerDoc("v1", new Info { Title = "NetCore", Version = "v1" });
                options.DescribeAllEnumsAsStrings();
            });

            // services.AddNodeServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddFile(Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Configure CORS
            app.UseCors(options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            });

            // Custom error handling middleware to support status codes for specific exceptions
            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();

            if (env.IsDevelopment() || env.IsEnvironment("Test"))
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                app.UseSwagger();

                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCore V1");
                    c.DocExpansion("none");
                });
            }

            app.UseMvc();

            //var periodicService = new PeriodicService(app.ApplicationServices);
            //periodicService.Start();
        }

        private void ConfigureDataRepositories(IServiceCollection services)
        {
            // Register DB contexts
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("AppDB")));
            //services.AddScoped(ReadOnlyDbContextFactory.Instance(Configuration));

            // Register database scope
            services.AddScoped<IDatabaseScope, AppDatabaseScope>();

            // Register repositories
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IGenericRepository<Country>, GenericRepository<Country>>();
            services.AddScoped<IGenericRepository<Currency>, GenericRepository<Currency>>();
            services.AddScoped<IGenericRepository<Role>, GenericRepository<Role>>();
            services.AddScoped<IGenericRepository<EmailTemplate>, GenericRepository<EmailTemplate>>();

            // Register readonly repositories
            services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            services.AddScoped<IGenericRepository<Country>, GenericRepository<Country>>();
            services.AddScoped<IGenericRepository<Currency>, GenericRepository<Currency>>();
            services.AddScoped<IGenericRepository<Role>, GenericRepository<Role>>();
            services.AddScoped<IGenericRepository<EmailTemplate>, GenericRepository<EmailTemplate>>();
            services.AddScoped<IGenericRepository<UserCountry>, GenericRepository<UserCountry>>();
        }

        private void ConfigureBusinessServices(IServiceCollection services)
        {
            services.AddSingleton(new EmailSender(Configuration.GetSection("EmailSenderOptions")));

            services.AddSingleton<CryptographyService>();

            services.AddScoped<SeedService>();

            services.AddScoped<AuthenticationService>();
            services.AddScoped<CurrencyService>();
            services.AddScoped<CountryService>();
            services.AddScoped<RoleService>();
            services.AddScoped<UserService>();
            services.AddScoped<EmailService>();
        }
    }
}
