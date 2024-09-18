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
    public class UsuarioController : ControllerBase
    {

        private readonly IUsuarioService _usuarioService;
        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Route("Lista")]
        public async Task<IActionResult> Lista()
        {
            var response = new Response<List<UsuarioDTO>>();

            try{
                response.status = true;
                response.value = await _usuarioService.Lista();

            } catch(Exception ex) {
                response.status = false;
                response.msg = ex.Message;
            }

            return Ok(response);
        }


        [HttpPost]
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDTO login)
        {
            var response = new Response<SesionDTO>();

            try{
                response.status = true;
                response.value = await _usuarioService.ValidarCredenciales(login.Correo,login.Clave);

            } catch(Exception ex) {
                response.status = false;
                response.msg = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] UsuarioDTO usuario)
        {
            var response = new Response<UsuarioDTO>();

            try{
                response.status = true;
                response.value = await _usuarioService.Crear(usuario);

            } catch(Exception ex) {
                response.status = false;
                response.msg = ex.Message;
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] UsuarioDTO usuario)
        {
            var response = new Response<bool>();

            try{
                response.status = true;
                response.value = await _usuarioService.Editar(usuario);

            } catch(Exception ex) {
                response.status = false;
                response.msg = ex.Message;
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = new Response<bool>();

            try{
                response.status = true;
                response.value = await _usuarioService.Eliminar(id);

            } catch(Exception ex) {
                response.status = false;
                response.msg = ex.Message;
            }

            return Ok(response);
        }


    }
}