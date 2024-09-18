using System;
using System.Collections.Generic;
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
    public class MenuService:IMenuService
    {

        private readonly IGenericRepository<Usuario> _usuarioRepository;
        private readonly IGenericRepository<MenuRol> _menuRolRepository;
        private readonly IGenericRepository<Menu> _menuRepository;
        private readonly IMapper _mapper;

        public MenuService(IGenericRepository<Usuario> usuarioRepository, IGenericRepository<MenuRol> menuRolRepository, IGenericRepository<Menu> menuRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _menuRolRepository = menuRolRepository;
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public async Task<List<MenuDTO>> Lista(int idUsuario)
        {
            IQueryable<Usuario> tbUsuario = await _usuarioRepository.Consultar(u => u.IdUsuario == idUsuario); 
            IQueryable<MenuRol> tbMenuRol = await _menuRolRepository.Consultar();
            IQueryable<Menu> tbMenu = await _menuRepository.Consultar();

            try{
                IQueryable<Menu> tbResultado = (from u in tbUsuario 
                                                join mr in tbMenuRol on u.IdRol equals mr.IdRol
                                                join m in tbMenu on mr.IdMenu equals m.IdMenu
                                                select m).AsQueryable();

                var listaMenus = tbResultado.ToList();
                
                var menuDTOMap = _mapper.Map<List<MenuDTO>>(listaMenus);

                return menuDTOMap;

            } catch {
                throw;
            }
        }
        
    }
}