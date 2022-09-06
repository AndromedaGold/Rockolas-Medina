using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;


namespace RockolasMedina.Models
{
    [DataContract(IsReference = true)]
    public class Rockola
    {
        public int IDrockola { get; set; }
        public string motherb { get; set; }
        public string ram { get; set; }
        public string procesador { get; set; }
        public string fuentep { get; set; }
        public string hddcapacidad { get; set; }
        public string monitor { get; set; }
        public string teclado { get; set; }
        public DateTime fechaFabricacion { get; set; }
        public int Activa { get; set; }
        public int IDruta { get; set; }
    }
}