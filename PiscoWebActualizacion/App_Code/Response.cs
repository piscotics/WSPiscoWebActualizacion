using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PiscoWebActualizacion.App_Code
{
    public class Response
    {
        private String resultText;
        public string ResultText { set; get; }
        public Response()
        {
            resultText = "El servicio no esta disponible en este momento por favor contacte al proveedor.";
        }

        public Response(string strResultado)
        {
            resultText = strResultado;
        }
    }
}