using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace CafeEPOS.Shared.Services
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddEposServices(this IServiceCollection services)
        {
            services.AddHttpClient("api", o =>
            {
                o.BaseAddress = new Uri("https://web.socem.plymouth.ac.uk/comp3000/tcard-api/");
            });

            services.AddTransient<StatsService>();
            services.AddTransient<AdminService>();
            services.AddTransient<AuthenticationService>();
            services.AddTransient<CategoryService>();
            services.AddTransient<MenuService>();
            services.AddTransient<OrderService>();


            return services;
        }
    }
}
