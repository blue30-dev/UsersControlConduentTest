using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UsersControlConduentTest.Models
{
    public class UserModel
    {
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string FechaNacimientoStr { get; set; }
        public int Grado { get; set; }
        public string Grupo { get; set; }
        public int Calificacion { get; set; }
        public string Clave { get; set; }
        public string ClaveModificada { get; set; }
    }
}