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
    public class ProductoService:IProductoService
    {

        private readonly IGenericRepository<Producto> _productoRepository;
        private readonly IMapper _mapper;
        public ProductoService(IGenericRepository<Producto> productoRepository, IMapper mapper)
        {
            _productoRepository = productoRepository;
            _mapper = mapper;
        }
        
        public async Task<List<ProductoDTO>> Lista()
        {
            try{
                var query = await _productoRepository.Consultar();
                var listaProducto = query.Include(c => c.IdCategoriaNavigation).ToList();
                
                var productoDTOMap = _mapper.Map<List<ProductoDTO>>(listaProducto.ToList());

                return productoDTOMap;
                
            } catch {
                throw;
            }
        }

        public async Task<ProductoDTO> Crear(ProductoDTO modelo)
        {
            try{
                var productoModelo = _mapper.Map<Producto>(modelo);
                var productoCreado = await _productoRepository.Crear(productoModelo);

                if(productoCreado.IdProducto == 0)
                    throw new TaskCanceledException("No se pudo crear");
                
                var productoDTOMap = _mapper.Map<ProductoDTO>(productoCreado);
                
                return productoDTOMap;

            } catch {
                throw;
            }
        }

        public async Task<bool> Editar(ProductoDTO modelo)
        {
            try{
                var productoModelo = _mapper.Map<Producto>(modelo);
                var productoEncontrado = await _productoRepository.Obtener(p => p.IdProducto == productoModelo.IdProducto);

                if(productoEncontrado == null)
                    throw new TaskCanceledException("El usuario no existe");

                productoEncontrado.Nombre = productoModelo.Nombre;
                productoEncontrado.IdCategoria = productoModelo.IdCategoria;
                productoEncontrado.Stock = productoModelo.Stock;
                productoEncontrado.Precio = productoModelo.Precio;
                productoEncontrado.EsActivo = productoModelo.EsActivo;

                bool response = await _productoRepository.Editar(productoEncontrado);

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
                var productoEncontrado = await _productoRepository.Obtener(p => p.IdProducto == id);

                if(productoEncontrado == null)
                    throw new TaskCanceledException("El producto no existe");
                
                bool response = await _productoRepository.Eliminar(productoEncontrado);

                if(!response)
                    throw new TaskCanceledException("No se pudo eliminar.");
                
                return response;

            } catch {
                throw;
            }
        }

    }
}