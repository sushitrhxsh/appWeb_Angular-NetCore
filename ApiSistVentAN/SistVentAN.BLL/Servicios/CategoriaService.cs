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
    public class CategoriaService:ICategoriaService
    {

        private readonly IGenericRepository<Categoria> _categoriaRepository;
        private readonly IMapper _mapper;
        public CategoriaService(IGenericRepository<Categoria> categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoriaDTO>> Lista()
        {
            try{
                var listaCategorias = await _categoriaRepository.Consultar();

                var listaMap = _mapper.Map<List<CategoriaDTO>>(listaCategorias.ToList());

                return listaMap;

            } catch {
                throw;
            }
        }

    }
}