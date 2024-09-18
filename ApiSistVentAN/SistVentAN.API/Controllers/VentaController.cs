using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using SistVentAN.BLL.Servicios.Contrato;
using SistVentAN.DTO;
using SistVentAN.API.Utilidad;

namespace SistVentAN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentaController : ControllerBase
    {

        private readonly IVentaService _ventaService;
        public VentaController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [HttpGet]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDTO venta)
        {
            var response = new Response<VentaDTO>();

            try{
                response.status = true;
                response.value = await _ventaService.Registrar(venta);

            } catch(Exception ex) {
                response.status = false;
                response.msg = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("Historial")] // /{buscarPor:string}/{numeroVenta:int}/{fechaInicio:string}/{fechaFinal:string}
        public async Task<IActionResult> Historial(string buscarPor, string? numeroVenta, string? fechaInicio, string? fechaFinal)
        {
            var response = new Response<List<VentaDTO>>();

            numeroVenta = numeroVenta is null ? "":numeroVenta;
            fechaInicio = fechaInicio is null ? "":fechaInicio;
            fechaFinal = fechaFinal is null ? "":fechaFinal;

            try{
                response.status = true;
                response.value = await _ventaService.Historial(buscarPor,numeroVenta,fechaInicio,fechaFinal);

            } catch(Exception ex) {
                response.status = false;
                response.msg = ex.Message;
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("Reporte")] // /{fechaInicio:string}/{fechaFinal:string}
        public async Task<IActionResult> Reporte( string? fechaInicio, string? fechaFinal)
        {
            var response = new Response<List<ReporteDTO>>();

            fechaInicio = fechaInicio is null ? "":fechaInicio;
            fechaFinal = fechaFinal is null ? "":fechaFinal;

            try{
                response.status = true;
                response.value = await _ventaService.Reporte(fechaInicio,fechaFinal);

            } catch(Exception ex) {
                response.status = false;
                response.msg = ex.Message;
            }

            return Ok(response);
        }

    }
}