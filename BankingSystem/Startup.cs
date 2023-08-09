using Microsoft.EntityFrameworkCore;
using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Banking.Services.Services;
using Banking.API.Controllers;

namespace BankingSyst.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext
            services.AddDbContext<BankingDbContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("BankingDbConnection"),
                b => b.MigrationsAssembly("Banking.API")
            ));

            services.Configure<AlphaVantageSettings>(Configuration.GetSection("AlphaVantage"));
            services.Configure<ExchangeApiSettings>(Configuration.GetSection("ExchangeApiSettings"));
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.Configure<TokenSettings>(Configuration.GetSection("TokenSettings"));

            services.AddTransient<ILoanCalculatorService, LoanCalculatorService>();
            services.AddScoped<ILoanEligibilityService, LoanEligibilityService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<INotificationService, NotificationService>();



            services.AddControllers();
            services.AddMemoryCache();
            services.AddHttpClient();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://your_frontend_domain.com")
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthentication(); // Ensure this is placed before UseAuthorization
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors();
        }
    }
}
