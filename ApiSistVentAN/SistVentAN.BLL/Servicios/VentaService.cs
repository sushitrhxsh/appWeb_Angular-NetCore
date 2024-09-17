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
    public class VentaService:IVentaService
    {

        private readonly IVentaRepository _ventaRepository;
        private readonly IGenericRepository<DetalleVenta> _detalleVentaRepository;
        private readonly IMapper _mapper;
        public VentaService(IVentaRepository ventaRepository, IGenericRepository<DetalleVenta> detalleVentaRepository, IMapper mapper)
        {
            _ventaRepository = ventaRepository;
            _detalleVentaRepository = detalleVentaRepository;
            _mapper = mapper;
        }

        public async Task<VentaDTO> Registrar(VentaDTO modelo)
        {
            try{

                var ventaModelo = _mapper.Map<Venta>(modelo);
                var ventaGenerada = await _ventaRepository.Registrar(ventaModelo);

                if(ventaGenerada.IdVenta == 0)
                    throw new TaskCanceledException("No se pudo crear");
                
                return _mapper.Map<VentaDTO>(ventaGenerada);

            } catch {
                throw;
            }
        }

        public async Task<List<VentaDTO>> Historial(string buscarPor, string numeroVenta, string fechaInicio, string fechaFinal)
        {
            IQueryable<Venta> query = await _ventaRepository.Consultar();
            var listaResultado = new List<Venta>();

           try{
                if(buscarPor == "fecha") {
                    DateTime fechInicio = DateTime.ParseExact(fechaInicio,"dd/MM/   yyyy", new CultureInfo("es-MX"));
                    DateTime fechFinal = DateTime.ParseExact(fechaFinal,"dd/MM/yyyy", new CultureInfo("es-MX"));

                    listaResultado = await query.Where(v => v.FechaRegistro.Value.Date >= fechInicio.Date && v.FechaRegistro.Value.Date <= fechFinal.Date)
                    .Include(dv => dv.DetalleVenta)
                    .ThenInclude(p => p.IdProductoNavigation)
                    .ToListAsync();

                } else {
                    listaResultado = await query.Where(v => v.NumeroDocumento == numeroVenta)
                    .Include(dv => dv.DetalleVenta)
                    .ThenInclude(p => p.IdProductoNavigation)
                    .ToListAsync();
                }

            } catch {
                throw;
            }

            return _mapper.Map<List<VentaDTO>>(listaResultado);
        }

        public async Task<List<ReporteDTO>> Reporte(string fechaInicio, string fechaFinal)
        {
            IQueryable<DetalleVenta> query = await _detalleVentaRepository.Consultar();
            var listaResultado = new List<DetalleVenta>();

            try{
                DateTime fechInicio = DateTime.ParseExact(fechaInicio,"dd/MM/   yyyy", new CultureInfo("es-MX"));
                DateTime fechFinal = DateTime.ParseExact(fechaFinal,"dd/MM/yyyy", new CultureInfo("es-MX"));

                listaResultado = await query
                .Include(p => p.IdProductoNavigation)
                .Include(v => v.IdVentaNavigation)
                .Where(dv => dv.IdVentaNavigation.FechaRegistro.Value.Date >= fechInicio.Date && dv.IdVentaNavigation.FechaRegistro.Value.Date <= fechFinal.Date)
                .ToListAsync();

            } catch {
                throw;
            }

            return _mapper.Map<List<ReporteDTO>>(listaResultado);
        }

    }
}