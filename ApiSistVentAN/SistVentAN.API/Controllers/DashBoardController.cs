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
    public class DashBoardController : ControllerBase
    {

        private readonly IDashBoardService _dashboardService;
        public DashBoardController(IDashBoardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Route("Resumen")]
        public async Task<IActionResult> Resumen()
        {
            var response = new Response<DashBoardDTO>();

            try{
                response.status = true;
                response.value = await _dashboardService.Resumen();

            } catch(Exception ex) {
                response.status = false;
                response.msg = ex.Message;
            }

            return Ok(response);
        }

    }
}