using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RockolasMedina.Models
{
    public class Reparacion
    {
        public int IDreparacion { get; set; }
        public string ParteNombre { get; set; }
        public string Ruta { get; set; }
        public string TecnicoNombre { get; set; }
        public DateTime FechaReparacion { get; set; }
        public string Comentario { get; set; }
        public int Activa { get; set; }
        public int IDrockola { get; set; }
    }
}