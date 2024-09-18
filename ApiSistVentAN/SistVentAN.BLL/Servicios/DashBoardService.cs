using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistVentAN.BLL.Servicios.Contrato;
using SistVentAN.DAL.Repositorios.Contrato;
using SistVentAN.DTO;
using SistVentAN.Model;

namespace SistVentAN.BLL.Servicios
{
    public class DashBoardService:IDashBoardService
    {

        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IMapper _mapper;
        public DashBoardService(IVentaRepository ventaRepository, IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _productoRepository = productoRepository;
            _mapper = mapper;
        }

        private IQueryable<Venta> RetornarVentas(IQueryable<Venta> tablaVenta, int restarCantidadDias)
        {
            DateTime? ultimaFecha = tablaVenta.OrderByDescending(v => v.FechaRegistro).Select(v => v.FechaRegistro).First();

            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);

            return tablaVenta.Where(v => v.FechaRegistro.Value.Date >= ultimaFecha.Value.Date);
        }

        private async Task<int> TotalVentasUltimaSemana()
        {
            int total = 0;
            IQueryable<Venta> ventaQuery = await _ventaRepository.Consultar();

            if(ventaQuery.Count() > 0) {
                var tablaVenta = RetornarVentas(ventaQuery,-7);

                total = tablaVenta.Count();
            } 

            return total;
        }

        private async Task<string> TotalIngresosUltimaSemana()
        {
            decimal resultado = 0;
            IQueryable<Venta> ventaQuery = await _ventaRepository.Consultar();

            if(ventaQuery.Count() > 0){
                var tablaVenta = RetornarVentas(ventaQuery, -7);

                resultado = tablaVenta.Select(v => v.Total).Sum(v => v.Value);
            }

            return Convert.ToString(resultado,new CultureInfo("es-MX"));

        }

        private async Task<int> TotalProductos()
        {
            IQueryable<Producto> productoQuery = await _productoRepository.Consultar();
            int total = productoQuery.Count();

            return total;
        }

        private async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            Dictionary<string,int> resultado = new Dictionary<string,int>();

            IQueryable<Venta> ventaQuery = await _ventaRepository.Consultar();
            if(ventaQuery.Count() > 0) {
                var tablaVenta = RetornarVentas(ventaQuery, -7);

                resultado = tablaVenta
                .GroupBy(v => v.FechaRegistro.Value.Date)
                .OrderBy(g => g.Key)
                .Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                .ToDictionary(keySelector:r => r.fecha, elementSelector:r => r.total);
            }

            return resultado;
        }


        public async Task<DashBoardDTO> Resumen()
        {
            DashBoardDTO vmDashBoard = new DashBoardDTO();

            try{
                vmDashBoard.TotalVentas = await TotalVentasUltimaSemana();
                vmDashBoard.TotalIngresos = await TotalIngresosUltimaSemana();
                vmDashBoard.TotalProductos = await TotalProductos();

                List<VentaSemanaDTO> listaVentaSemana = new List<VentaSemanaDTO>();

                foreach(KeyValuePair<string,int> item in await VentasUltimaSemana())
                {
                    listaVentaSemana.Add(new VentaSemanaDTO(){
                        Fecha = item.Key,
                        Total = item.Value
                    });
                }

                vmDashBoard.VentasUltimaSemana = listaVentaSemana;

            } catch {
                throw;
            }

            return vmDashBoard;
        }

    }
}