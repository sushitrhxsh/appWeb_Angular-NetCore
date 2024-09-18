using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistVentAN.API.Utilidad
{
    public class Response<T>
    {
        public bool status { get; set; }
        public T value { get; set; }
        public string msg { get; set; }
    }
}