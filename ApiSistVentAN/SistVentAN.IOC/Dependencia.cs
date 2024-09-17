using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistVentAN.DAL.DBContext;

using SistVentAN.DAL.Repositorios.Contrato;
using SistVentAN.DAL.Repositorios;

using SistVentAN.Utility;
using SistVentAN.BLL.Servicios.Contrato;
using SistVentAN.BLL.Servicios;

namespace SistVentAN.IOC
{
    public static class Dependencia
    {
        public static void InyectarDependencias(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DbventAnContext>(options => {
                options.UseSqlServer(configuration.GetConnectionString("cadenaSQL"));
            });

            services.AddTransient(typeof(IGenericRepository<>),typeof(GenericRepository<>));    // SistVentAN.DAL/Repositorios/Contrato/IGenericRepository - SistVentAN.DAL/Repositorios/GenericRepository
            services.AddScoped<IVentaRepository, VentaRepository>();    // SistVentAN.DAL/Repositorios/Contrato/IVentaRepository- SistVentAN.DAL/Repositorios/VentaRepository

            services.AddAutoMapper(typeof(AutoMapperProfile));  // SistVentAN.Utility/AutoMapperProfile

            services.AddScoped<IRolService,RolService>();
            services.AddScoped<IUsuarioService,UsuarioService>();
            services.AddScoped<ICategoriaService,CategoriaService>();
            services.AddScoped<IProductoService,ProductoService>();
            services.AddScoped<IVentaService,VentaService>();
            services.AddScoped<IDashBoardService,DashBoardService>();
            services.AddScoped<IMenuService,MenuService>();

        }
        
        
    }
}