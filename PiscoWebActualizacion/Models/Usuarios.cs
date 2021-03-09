using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PiscoWebActualizacion.Models
{
    public class Usuarios
    {
        public string Username { get; set; }
        public string Pwduser { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Dbrol { get; set; }
        public string Pwdrol { get; set; }
        public string Usuariomaster { get; set; }
        public string Cargo { get; set; }
        public string Estado { get; set; }
    }
}