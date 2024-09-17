using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistVentAN.DTO
{
    public class DashBoardDTO
    {
        public int? TotalVentas { get; set; }
        public string? TotalIngresos { get; set; }
        public int TotalProductos { get; set; }
        public List<VentaSemanaDTO> VentasUltimaSemana { get; set; }
    }
}