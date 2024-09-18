using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using AutoMapper;
using SistVentAN.BLL.Servicios.Contrato;
using SistVentAN.DAL.Repositorios.Contrato;
using SistVentAN.DTO;
using SistVentAN.Model;

namespace SistVentAN.BLL.Servicios
{
    public class UsuarioService:IUsuarioService
    {

        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IMapper _mapper;
        public UsuarioService(IGenericRepository<Usuario> usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<List<UsuarioDTO>> Lista()
        {
            try{
                var query = await _usuarioRepository.Consultar();
                var listaUsuario = query.Include(r => r.IdRolNavigation).ToList(); // r = rol

                var usuarioDTOMap = _mapper.Map<List<UsuarioDTO>>(listaUsuario);

                return usuarioDTOMap;

            } catch {
                throw;
            }
        }

        public async Task<SesionDTO> ValidarCredenciales(string correo, string clave)
        {
            try{
                var query = await _usuarioRepository.Consultar(u => u.Correo == correo && u.Clave == clave); // u = usuario
                
                if(query.FirstOrDefault() == null)
                    throw new TaskCanceledException("El usuario no existe");
                
                Usuario devolverUsuario = query.Include(r => r.IdRolNavigation).First();

                var sesionDTOMap = _mapper.Map<SesionDTO>(devolverUsuario);

                return sesionDTOMap;

            } catch {
                throw;
            }
        }

        public async Task<UsuarioDTO> Crear(UsuarioDTO modelo)
        {
            try{
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                var usuarioCreado = await _usuarioRepository.Crear(usuarioModelo);

                if(usuarioCreado.IdUsuario == 0)
                    throw new TaskCanceledException("El usuario no se pudo crear.");
                
                var query = await _usuarioRepository.Consultar(u => u.IdUsuario == usuarioCreado.IdUsuario);

                usuarioCreado = query.Include(r => r.IdRolNavigation).First();

                var usuarioDTOMap = _mapper.Map<UsuarioDTO>(usuarioCreado);

                return usuarioDTOMap;

            } catch {
                throw;
            }
        }

        public async Task<bool> Editar(UsuarioDTO modelo)
        {
            try{
                var usuarioModelo = _mapper.Map<Usuario>(modelo);
                var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == usuarioModelo.IdUsuario);

                if(usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                usuarioEncontrado.NombreCompleto = usuarioModelo.NombreCompleto;
                usuarioEncontrado.Correo = usuarioModelo.Correo;
                usuarioEncontrado.IdRol = usuarioModelo.IdRol;
                usuarioEncontrado.Clave = usuarioModelo.Clave;
                usuarioEncontrado.EsActivo = usuarioModelo.EsActivo;

                bool response = await _usuarioRepository.Editar(usuarioEncontrado);

                if(!response)
                    throw new TaskCanceledException("No se pudo editar.");
                
                return response;

            } catch {
                throw;
            }
        }

        public async Task<bool> Eliminar(int id)
        {
            try{
                var usuarioEncontrado = await _usuarioRepository.Obtener(u => u.IdUsuario == id);

                if(usuarioEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");
                
                bool response = await _usuarioRepository.Eliminar(usuarioEncontrado);

                if(!response)
                    throw new TaskCanceledException("No se pudo eliminar.");
                
                return response;

            } catch {
                throw;
            }
        }
        
    }
}