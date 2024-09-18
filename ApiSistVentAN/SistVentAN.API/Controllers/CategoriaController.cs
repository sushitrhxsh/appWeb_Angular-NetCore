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
    public class CategoriaController : ControllerBase
    {

        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var response = new Response<List<CategoriaDTO>>();

            try{
                response.status = true;
                response.value = await _categoriaService.Lista();

            } catch(Exception ex) {
                response.status = false;
                response.msg = ex.Message;
            }

            return Ok(response);
        }

    }
}