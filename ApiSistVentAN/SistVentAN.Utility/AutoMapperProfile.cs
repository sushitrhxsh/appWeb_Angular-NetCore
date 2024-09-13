using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using SistVentAN.DTO;
using SistVentAN.Model;

namespace SistVentAN.Utility
{
    public class AutoMapperProfile:Profile
    {
        public AutoMapperProfile()
        {
            #region Rol
            CreateMap<Rol,RolDTO>().ReverseMap();
            #endregion Rol

            #region Menu
            CreateMap<Menu,MenuDTO>().ReverseMap();
            #endregion Menu

            #region Usuario
            CreateMap<Usuario,UsuarioDTO>()
                .ForMember(dest => dest.RolDescripcion, opt => opt.MapFrom(o => o.IdRolNavigation.Nombre))
                .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(o => o.EsActivo == true ? 1:0));
            
            CreateMap<Usuario,SesionDTO>()
                .ForMember(dest => dest.RolDescripcion, opt => opt.MapFrom(o => o.IdRolNavigation.Nombre));
            
            CreateMap<UsuarioDTO,Usuario>()
                .ForMember(dest => dest.IdRolNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(o => o.EsActivo == 1 ? true:false));
            #endregion Usuario
            
            #region Categoria
            CreateMap<Categoria,CategoriaDTO>().ReverseMap();
            #endregion Categoria

            #region Producto
            CreateMap<Producto,ProductoDTO>()
                .ForMember(dest => dest.DescripcionCategoria, opt => opt.MapFrom(o => o.IdCategoriaNavigation.Nombre))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(o => Convert.ToString(o.Precio.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(o => o.EsActivo == true ? 1:0));

            CreateMap<ProductoDTO,Producto>()
                .ForMember(dest => dest.IdCategoriaNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(o => Convert.ToDecimal(o.Precio, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.EsActivo, opt => opt.MapFrom(o => o.EsActivo == 1 ? true:false));
            #endregion Producto

            #region Venta
            CreateMap<Venta,VentaDTO>()
                .ForMember(dest => dest.TotalTexto, opt => opt.MapFrom(o => Convert.ToString(o.Total.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(o => o.FechaRegistro.Value.ToString("dd/MM/yyyy")));

             CreateMap<VentaDTO,Venta>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(o => Convert.ToDecimal(o.TotalTexto, new CultureInfo("es-MX"))));
            #endregion Venta

            #region DetalleVenta
            CreateMap<DetalleVenta,DetalleVentaDTO>()
                .ForMember(dest => dest.DescripcionProducto, opt => opt.MapFrom(o => o.IdProductoNavigation.Nombre))
                .ForMember(dest => dest.PrecioTexto, opt => opt.MapFrom(o => Convert.ToString(o.Precio.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.TotalTexto, opt => opt.MapFrom(o => Convert.ToString(o.Total.Value, new CultureInfo("es-MX"))));
            
            CreateMap<DetalleVentaDTO,DetalleVenta>()
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(o => Convert.ToDecimal(o.PrecioTexto, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(o => Convert.ToDecimal(o.TotalTexto, new CultureInfo("es-MX"))));
                
            #endregion DetalleVenta

            #region Reporte
            CreateMap<DetalleVenta,ReporteDTO>()
                .ForMember(dest => dest.FechaRegistro, opt => opt.MapFrom(o => o.IdVentaNavigation.FechaRegistro.Value.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.NumeroDocumento, opt => opt.MapFrom(o => o.IdVentaNavigation.NumeroDocumento))
                .ForMember(dest => dest.TipoPago, opt => opt.MapFrom(o => o.IdVentaNavigation.TipoPago))
                .ForMember(dest => dest.TotalVenta, opt => opt.MapFrom(o => Convert.ToDecimal(o.IdVentaNavigation.Total.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.Producto, opt => opt.MapFrom(o => o.IdProductoNavigation.Nombre))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(o => Convert.ToDecimal(o.Precio.Value, new CultureInfo("es-MX"))))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(o => Convert.ToDecimal(o.Total.Value, new CultureInfo("es-MX"))));
            #endregion Reporte

        }
        
    }
}