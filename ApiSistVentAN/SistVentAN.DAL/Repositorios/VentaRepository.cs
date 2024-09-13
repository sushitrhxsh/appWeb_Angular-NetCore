using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistVentAN.DAL.DBContext;
using SistVentAN.DAL.Repositorios.Contrato;
using SistVentAN.Model;

namespace SistVentAN.DAL.Repositorios
{
    public class VentaRepository:GenericRepository<Venta>, IVentaRepository
    {

        private readonly DbventAnContext _dbcontext;
        public VentaRepository(DbventAnContext dbcontext):base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<Venta> Registrar(Venta modelo)
        {
            Venta ventaGenerada = new Venta();
            
            using (var transaction = _dbcontext.Database.BeginTransaction()){
                try{
                    foreach(DetalleVenta dv in modelo.DetalleVenta)
                    {
                        Producto productoEncontrado = _dbcontext.Productos.Where(p => p.IdProducto == dv.IdProducto).First();

                        productoEncontrado.Stock = productoEncontrado.Stock - dv.Cantidad;

                        _dbcontext.Productos.Update(productoEncontrado);

                    }
                    await _dbcontext.SaveChangesAsync();

                    NumeroDocumento correlativo = _dbcontext.NumeroDocumentos.First();

                    correlativo.UltimoNumero = correlativo.UltimoNumero + 1;
                    correlativo.FechaRegistro = DateTime.Now;

                    _dbcontext.NumeroDocumentos.Update(correlativo);
                    await _dbcontext.SaveChangesAsync();

                    int cantidadDigitos = 0;
                    string ceros = string.Concat(Enumerable.Repeat("0",cantidadDigitos));
                    string numeroVenta = ceros + correlativo.UltimoNumero.ToString();

                    numeroVenta = numeroVenta.Substring(numeroVenta.Length - cantidadDigitos);

                    modelo.NumeroDocumento = numeroVenta;
                    await _dbcontext.Venta.AddAsync(modelo);
                    await _dbcontext.SaveChangesAsync();

                    ventaGenerada = modelo;

                    transaction.Commit();

                } catch {
                    transaction.Rollback();
                    throw;
                }
            }  

            return ventaGenerada;
        }

    }
}