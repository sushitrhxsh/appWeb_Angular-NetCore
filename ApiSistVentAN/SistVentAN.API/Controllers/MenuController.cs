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
    public class MenuController : ControllerBase
    {

        private readonly IMenuService _menuService;
        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista(int idUsuario)
        {
            var response = new Response<List<MenuDTO>>();

            try{
                response.status = true;
                response.value = await _menuService.Lista(idUsuario);

            } catch(Exception ex) {
                response.status = false;
                response.msg = ex.Message;
            }

            return Ok(response);
        }

    }
}