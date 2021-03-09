using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PiscoWebActualizacion.Models
{
    public class EnviarSms
    {
        public string Celular { get; set; }
        public string Mensaje { get; set; }
        public string Apikey { get; set; }
        public string Apisecret { get; set; }
    }
}