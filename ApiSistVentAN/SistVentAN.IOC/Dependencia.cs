using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistVentAN.DAL.DBContext;

namespace SistVentAN.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbventAnContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));
            });
        }
        
    }
}