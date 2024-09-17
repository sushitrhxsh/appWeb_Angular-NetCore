using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SistVentAN.DTO;

namespace SistVentAN.BLL.Servicios.Contrato
{
    public interface IDashBoardService
    {
        Task<DashBoardDTO> Resumen();  
    }
}