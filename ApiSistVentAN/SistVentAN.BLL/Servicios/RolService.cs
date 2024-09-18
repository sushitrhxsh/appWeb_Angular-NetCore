using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using SistVentAN.BLL.Servicios.Contrato;
using SistVentAN.DAL.Repositorios.Contrato;
using SistVentAN.DTO;
using SistVentAN.Model;

namespace SistVentAN.BLL.Servicios
{
    public class RolService:IRolService
    {

        private readonly IGenericRepository<Rol> _rolRepository;
        private readonly IMapper _mapper;
        public RolService(IGenericRepository<Rol> rolRepository, IMapper mapper)
        {
            _rolRepository = rolRepository;
            _mapper = mapper;
        }

        public async Task<List<RolDTO>> Lista()
        {
            try{
                var listaRoles = await _rolRepository.Consultar();
                var rolDTOMap = _mapper.Map<List<RolDTO>>(listaRoles.ToList());

                return rolDTOMap;

            } catch {
                throw;
            }
        }

    }
}