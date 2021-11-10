using PiscoWebActualizacion.App_Code;
using PiscoWebActualizacion.Models;
using PiscoWebActualizacion.smspisco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

namespace PiscoWebActualizacion.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/Titulares")]

    public class TitularesController : ApiController
    {
        private Response response;
        AdasysConnection _Conect;
        AdasysConnection _ConectBeneficiarios;
        AdasysConnection _ConectUbicacion;
        AdasysConnection _ConectUsuario;
        AdasysConnection _ConectRegistros;
        AdasysConnection _ConectRegistrosCount;
        AdasysConnection _ConectTitular;



        public IHttpActionResult  EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("GetUbicacion")]
        public IHttpActionResult GetUbicacion(string dato, string departamento, string rutaBd)
        {
            if (dato == "")
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            int fila = 0;
            bool encontro = false;
            _Conect = new AdasysConnection();
            _Conect.FbConeccion(rutaBd);
            _Conect.StProcedure("P_TITULARESPARAMETROS");
            _Conect.StParameters("dato", dato);
            _Conect.StParameters("departamento", departamento);
            

            _Conect.ExecuteProcedure();

            int cantidad = cantidadUbicacion(dato,  departamento, rutaBd);
            Ubicacion[] objUbicacion = new Ubicacion[cantidad];

            while (_Conect.DataReader.Read())
            {
                encontro = true;
                objUbicacion[fila] = new Ubicacion();
                objUbicacion[fila].Dato = _Conect.DataReader["Datos"].ToString();
                fila = fila + 1;
            }
           
            if (encontro == true)
            {
                return Json(objUbicacion);
            }
            else
            {

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Sin Datos\" }]"));

            }
            // return Json(objTitulares);
        }

        private int cantidadUbicacion(string dato, string departamento, string rutaBd)
        {
            int cantidad = 0;
            try
            {

                _ConectUbicacion = new AdasysConnection();
                _ConectUbicacion.FbConeccion(rutaBd);
                _ConectUbicacion.StProcedure("P_TITULARESPARAMETROS");
                _ConectUbicacion.StParameters("dato", dato);
                _ConectUbicacion.StParameters("departamento", departamento);
            
                _ConectUbicacion.ExecuteProcedure();


                while (_ConectUbicacion.DataReader.Read())
                {

                    cantidad = cantidad + 1;

                }

            }
            catch (Exception e)
            {
                _ConectUbicacion.Conect.Close();
                string x = e.ToString();
            }
            return cantidad;
        }

        private int cantidadUsuarios(string rutaBd)
        {
            int cantidad = 0;
            try
            {

                _ConectUsuario = new AdasysConnection();
                _ConectUsuario.FbConeccion(rutaBd);
                _ConectUsuario.StProcedure("P_USUARIOSACTUALIZACION");
                _ConectUsuario.StParameters("usuario", "");
                _ConectUsuario.StParameters("passwordusuario", "");

                _ConectUsuario.ExecuteProcedure();


                while (_ConectUsuario.DataReader.Read())
                {

                    cantidad = cantidad + 1;

                }

            }
            catch (Exception e)
            {
                _ConectUsuario.Conect.Close();
                string x = e.ToString();
            }
            return cantidad;
        }

        [HttpGet]
        [Route("GetTitulares")]
        public IHttpActionResult Get(string cedula, string rutaBd)
        {
            if (cedula == "")
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            int fila = 0;
            bool encontro = false;
            DateTime fechatitular;
            _Conect = new AdasysConnection();
            _Conect.FbConeccion(rutaBd);
            _Conect.StProcedure("P_TITULARESACTUALIZACION");
            _Conect.StParameters("cedula", cedula);
            _Conect.ExecuteProcedure();

            Titulares[] objTitulares = new Titulares[1];
            if (_Conect.DataReader.Read())
            {

                objTitulares[fila] = new Titulares();


                if (_Conect.DataReader["IDPERSONA"].ToString() != "")
                {
                    encontro = true;
                }
                else
                {
                    encontro = false;
                }
                

               
                objTitulares[fila].Contrato = _Conect.DataReader["IDContrato"].ToString();
                objTitulares[fila].Cedula = _Conect.DataReader["IDPERSONA"].ToString();
                objTitulares[fila].Nombre1 = _Conect.DataReader["Nombres"].ToString();
                objTitulares[fila].Nombre2 = _Conect.DataReader["Nombre2"].ToString();
                objTitulares[fila].Apellido1 = _Conect.DataReader["Apellidos"].ToString();
                objTitulares[fila].Apellido2 = _Conect.DataReader["Apellido2"].ToString();
                objTitulares[fila].Direccion = _Conect.DataReader["DIRECCION"].ToString();
                objTitulares[fila].Departamento = _Conect.DataReader["DEPTO"].ToString();
                objTitulares[fila].Ciudad = _Conect.DataReader["Ciudad"].ToString();
                objTitulares[fila].Barrio = _Conect.DataReader["BOXCONTRA"].ToString();
                objTitulares[fila].Telefono = _Conect.DataReader["Telefono"].ToString();
                objTitulares[fila].Telefamiliar = _Conect.DataReader["CELULAR"].ToString();
                objTitulares[fila].Email = _Conect.DataReader["PARENTES"].ToString();
                string fnacimiento = _Conect.DataReader["FNACIMIENTO"].ToString(); 
                if (fnacimiento != "")
                {
                    fechatitular = Convert.ToDateTime(fnacimiento);
                    objTitulares[fila].FechaNacimiento = fechatitular.ToString("yyyy-MM-dd");
                }
                objTitulares[fila].Estado = _Conect.DataReader["Estado"].ToString();

                //fila = fila + 1;
            }
            else
            {
                encontro = false;
            }
            if (encontro == true)
            {
                return Json(objTitulares);
            }
            else
            {
              
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Sin Datos\" }]"));
               
            }
           // return Json(objTitulares);
        }

        //trae la informacion del crm de pisco 
        [HttpGet]
        [Route("GetInformacionPrincipal")]
        public IHttpActionResult GetInformacionPrincipal(string dominio)
        {
            if (dominio == "")
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            int fila = 0;
            bool encontro = false;
            _ConectTitular = new AdasysConnection();
            _ConectTitular.FbConeccionPrincipal();
            _ConectTitular.StProcedure("P_ACTUALIZACIONDOMINIO");
            _ConectTitular.StParameters("dominio", dominio);
            _ConectTitular.ExecuteProcedure();

            Principal[] objPrincipal = new Principal[1];
            if (_ConectTitular.DataReader.Read())
            {

                objPrincipal[fila] = new Principal();


                if (_ConectTitular.DataReader["IDENTIFICACION"].ToString() != "")
                {
                    encontro = true;
                    objPrincipal[fila].Estado = "1";
                }
                else
                {
                    encontro = false;
                    objPrincipal[fila].Estado = "0";
                }

                objPrincipal[fila].Identificacion = _ConectTitular.DataReader["IDENTIFICACION"].ToString();

                objPrincipal[fila].RutaBd = _ConectTitular.DataReader["RUTABD"].ToString();

                objPrincipal[fila].Ip = _ConectTitular.DataReader["IP"].ToString();

                objPrincipal[fila].RutaBdPrincipal = _ConectTitular.DataReader["IP"].ToString() + ":" + _ConectTitular.DataReader["RUTABD"].ToString();

                
               // _ConectTitular.MultiConsulta(objPrincipal[fila].Ip + ":" + objPrincipal[fila].RutaBd);

                // HttpContext.Current.Session["RutaBD"] = objPrincipal[fila].Ip + ":" + objPrincipal[fila].RutaBd;

                // _Conect.StrCadena1 = objPrincipal[fila].Ip +":"+ objPrincipal[fila].RutaBd;

            }
            else
            {
                encontro = false;
            }
            if (encontro == true)
            {
                return Json(objPrincipal);
            }
            else
            {

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Sin Datos\" }]"));

            }
           
        }


        [HttpGet]
        [Route("GetUsuario")]
        public IHttpActionResult GetUsuario(string usuario, string passwordusuario, string rutaBd)
        {
            if (usuario == "" && passwordusuario =="")
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            int fila = 0;
            bool encontro = false;
            DateTime fechatitular;
            _Conect = new AdasysConnection();
            _Conect.FbConeccion(rutaBd);
            _Conect.StProcedure("P_USUARIOSACTUALIZACION");
            _Conect.StParameters("usuario", usuario);
            _Conect.StParameters("passwordusuario", passwordusuario);
            _Conect.ExecuteProcedure();

           // , :, :, :, :, :pwdrol, :, :,  :estado
            Usuarios[] objUsuarios = new Usuarios[1];
            if (_Conect.DataReader.Read())
            {

                objUsuarios[fila] = new Usuarios();
                if (_Conect.DataReader["username"].ToString() != "")
                {
                    encontro = true;
                }
                else
                {
                    encontro = false;
                }
                objUsuarios[fila].Username = _Conect.DataReader["username"].ToString();
                objUsuarios[fila].Pwduser = _Conect.DataReader["pwduser"].ToString();
                objUsuarios[fila].Nombres = _Conect.DataReader["nombres"].ToString();
                objUsuarios[fila].Apellidos = _Conect.DataReader["apellidos"].ToString();
                objUsuarios[fila].Dbrol = _Conect.DataReader["dbrol"].ToString();
                objUsuarios[fila].Pwdrol = _Conect.DataReader["pwdrol"].ToString();
                string master = _Conect.DataReader["usuariomaster"].ToString();
                if (master == "1")
                {
                    objUsuarios[fila].Usuariomaster = "1";
                }
                else
                {
                    objUsuarios[fila].Usuariomaster = "0";
                }
                objUsuarios[fila].Cargo = _Conect.DataReader["cargo"].ToString();
                string estado = _Conect.DataReader["estado"].ToString();
                if (estado == "1")
                {
                    objUsuarios[fila].Estado = "1";
                }
                else
                {
                    objUsuarios[fila].Estado = "0";
                }

                objUsuarios[fila].Configuracion = _Conect.DataReader["configuracion"].ToString();

                //fila = fila + 1;
            }
            else
            {
                encontro = false;
            }
            if (encontro == true)
            {
                return Json(objUsuarios);
            }
            else
            {

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Sin Datos\" }]"));

            }
            // return Json(objTitulares);
        }


        [HttpGet]
        [Route("GetAllUsuario")]
        public IHttpActionResult GetAllUsuario(string rutaBd)
        {
           

            int fila = 0;
            bool encontro = false;
            DateTime fechatitular;
            _Conect = new AdasysConnection();
            _Conect.FbConeccion(rutaBd);
            _Conect.StProcedure("P_USUARIOSACTUALIZACION");
            _Conect.StParameters("usuario", "");
            _Conect.StParameters("passwordusuario", "");
            _Conect.ExecuteProcedure();

            int cantidad = cantidadUsuarios(rutaBd);
            // , :, :, :, :, :pwdrol, :, :,  :estado
            Usuarios[] objUsuarios = new Usuarios[cantidad];
            while (_Conect.DataReader.Read())
            {

                objUsuarios[fila] = new Usuarios();
                if (_Conect.DataReader["username"].ToString() != "")
                {
                    encontro = true;
                }
                else
                {
                    encontro = false;
                }
                objUsuarios[fila].Username = _Conect.DataReader["username"].ToString();
                objUsuarios[fila].Pwduser = _Conect.DataReader["pwduser"].ToString();
                objUsuarios[fila].Nombres = _Conect.DataReader["nombres"].ToString();
                objUsuarios[fila].Apellidos = _Conect.DataReader["apellidos"].ToString();
                objUsuarios[fila].Dbrol = _Conect.DataReader["dbrol"].ToString();
                objUsuarios[fila].Pwdrol = _Conect.DataReader["pwdrol"].ToString();
                string master = _Conect.DataReader["usuariomaster"].ToString();
                if (master == "1")
                {
                    objUsuarios[fila].Usuariomaster = "1";
                }
                else
                {
                    objUsuarios[fila].Usuariomaster = "0";
                }
                objUsuarios[fila].Cargo = _Conect.DataReader["cargo"].ToString();
                string estado = _Conect.DataReader["estado"].ToString();
                if (estado == "1")
                {
                    objUsuarios[fila].Estado = "1";
                }
                else
                {
                    objUsuarios[fila].Estado = "0";
                }

                fila = fila + 1;
            }
            
            if (encontro == true)
            {
                return Json(objUsuarios);
            }
            else
            {

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Sin Datos\" }]"));

            }
            // return Json(objTitulares);
        }


        //trae toda la informacion de las actualiaciones
        [HttpGet]
        [Route("GetAllActualizaciones")]
        public IHttpActionResult GetAllActuualizaciones(string fechadesde,string fechahasta, string usuarioRegistro , string estadoRegistro, string procesadoRegistro, string POSX, string POSY, string rutaBd)
        {


            int fila = 0;
            bool encontro = false;
            DateTime fechadesderegistro;
            DateTime fechahastaregistro;
            DateTime fechatitular;

            _ConectRegistros = new AdasysConnection();
            _ConectRegistros.FbConeccion(rutaBd);
            _ConectRegistros.StProcedure("P_REGISTROSACTUALIZACION");
            
            if (fechadesde != null)
            {
                fechadesderegistro = Convert.ToDateTime(fechadesde);
                _ConectRegistros.StParameters("fechadesde", fechadesderegistro.ToString("yyyy-MM-dd"));

            }
            else
            {
                _ConectRegistros.StParameters("fechadesde", null);
            }
            if (fechahasta != null)
            {
                fechahastaregistro = Convert.ToDateTime(fechahasta);
                _ConectRegistros.StParameters("fechahasta", fechahastaregistro.ToString("yyyy-MM-dd"));

            }
            else
            {
                _ConectRegistros.StParameters("fechahasta", null);
            }
            if (usuarioRegistro != null)
            {
               
                _ConectRegistros.StParameters("usuarioRegistro", usuarioRegistro);

            }
            else
            {
                _ConectRegistros.StParameters("usuarioRegistro", "");
            }

            if (estadoRegistro != null)
            {

                _ConectRegistros.StParameters("estadoRegistro", estadoRegistro);

            }
            else
            {
                _ConectRegistros.StParameters("estadoRegistro", "");
            }
            if (procesadoRegistro == "0")
            {

                _ConectRegistros.StParameters("procesadoRegistro", "0");

            }
            else
            {
                _ConectRegistros.StParameters("procesadoRegistro", "1");
            }
            _ConectRegistros.ExecuteProcedure();

            int cantidad = cantidadRegistrosActualizacion( fechadesde,  fechahasta,  usuarioRegistro,  estadoRegistro,  procesadoRegistro, POSX, POSY,rutaBd);

            Registros[] objRegistros = new Registros[cantidad];
            while (_ConectRegistros.DataReader.Read())
            {

                objRegistros[fila] = new Registros();
                if (_ConectRegistros.DataReader["idactualizacion"].ToString() != "")
                {
                    encontro = true;
                }
                else
                {
                    encontro = false;
                }
                objRegistros[fila].Idactualizacion = _ConectRegistros.DataReader["idactualizacion"].ToString();
                objRegistros[fila].Idpersona = _ConectRegistros.DataReader["idpersona"].ToString();
                objRegistros[fila].Nombre1 = _ConectRegistros.DataReader["nombres"].ToString();
                objRegistros[fila].Nombre2 = _ConectRegistros.DataReader["nombre2"].ToString();
                objRegistros[fila].Apellido1 = _ConectRegistros.DataReader["apellidos"].ToString();
                objRegistros[fila].Apellido2 = _ConectRegistros.DataReader["apellido2"].ToString();
                objRegistros[fila].Telefono = _ConectRegistros.DataReader["telefono"].ToString();
                objRegistros[fila].Telefamiliar = _ConectRegistros.DataReader["telefamiliar"].ToString();
                objRegistros[fila].Direccion = _ConectRegistros.DataReader["direccion"].ToString();
                objRegistros[fila].Departamento = _ConectRegistros.DataReader["departamento"].ToString();
                objRegistros[fila].Ciudad = _ConectRegistros.DataReader["ciudad"].ToString();
                objRegistros[fila].Estado = _ConectRegistros.DataReader["estado"].ToString();
                string tienemascota = _ConectRegistros.DataReader["tienemascota"].ToString();
                if (tienemascota == "1")
                {
                    objRegistros[fila].Tienemascota = "Si";
                }
                else
                {
                    objRegistros[fila].Tienemascota = "No";
                }
                string seguromascota = _ConectRegistros.DataReader["seguromascota"].ToString();
                if (seguromascota == "1")
                {
                    objRegistros[fila].Seguromascota = "Si";
                }
                else
                {
                    objRegistros[fila].Seguromascota = "No";
                }

                string TienePlanExequial = _ConectRegistros.DataReader["TienePlanExequial"].ToString();
                if (TienePlanExequial == "1")
                {
                    objRegistros[fila].TienePlanExequial = "Si";
                }
                else
                {
                    objRegistros[fila].TienePlanExequial = "No";
                }

                objRegistros[fila].EntidadPlanExequial = _ConectRegistros.DataReader["EntidadPlanExequial"].ToString();


                objRegistros[fila].Barrio = _ConectRegistros.DataReader["barrio"].ToString();

                string fechanacimiento = _ConectRegistros.DataReader["fechanacimiento"].ToString();
                if (fechanacimiento != "")
                {
                    fechatitular = Convert.ToDateTime(fechanacimiento);
                    objRegistros[fila].Fechanacimiento = fechatitular.ToString("yyyy-MM-dd");
                }

                objRegistros[fila].Email = _ConectRegistros.DataReader["email"].ToString();
                objRegistros[fila].Detalle = _ConectRegistros.DataReader["detalle"].ToString();
                string procesado = _ConectRegistros.DataReader["procesado"].ToString();
                if (procesado == "1")
                {
                    objRegistros[fila].Procesado = "Si";
                }
                else
                {
                    objRegistros[fila].Procesado = "No";
                }
                objRegistros[fila].Usuario = _ConectRegistros.DataReader["usuario"].ToString();

                string fechanow = _ConectRegistros.DataReader["fechanow"].ToString();
                if (fechanow != "")
                {
                    fechatitular = Convert.ToDateTime(fechanow);
                    objRegistros[fila].Fechanow = fechatitular.ToString("yyyy-MM-dd HH:MM:SS");
                }

                string fecharegistro = _ConectRegistros.DataReader["fecharegistro"].ToString();
                if (fecharegistro != "")
                {
                    fechatitular = Convert.ToDateTime(fecharegistro);
                    objRegistros[fila].Fecharegistro = fechatitular.ToString("yyyy-MM-dd");
                }

                objRegistros[fila].POSX = _ConectRegistros.DataReader["POSX"].ToString();
                objRegistros[fila].POSY = _ConectRegistros.DataReader["POSY"].ToString();

                fila = fila + 1;
            }
            
            if (encontro == true)
            {
                return Json(objRegistros);
            }
            else
            {

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Sin Datos\" }]"));

            }
            // return Json(objTitulares);
        }

        private int cantidadRegistrosActualizacion(string fechadesde, string fechahasta, string usuarioRegistro, string estadoRegistro, string procesadoRegistro , string POSX, string POSY, string rutaBd)
        {

            int cantidad = 0;
            try
            {


                DateTime fechadesderegistro;
                DateTime fechahastaregistro;
                DateTime fechatitular;

                _ConectRegistrosCount = new AdasysConnection();
                _ConectRegistrosCount.FbConeccion(rutaBd);
                _ConectRegistrosCount.StProcedure("P_REGISTROSACTUALIZACION");

                if (fechadesde != null)
                {
                    fechadesderegistro = Convert.ToDateTime(fechadesde);
                    _ConectRegistrosCount.StParameters("fechadesde", fechadesderegistro.ToString("yyyy-MM-dd"));

                }
                else
                {
                    _ConectRegistrosCount.StParameters("fechadesde", null);
                }
                if (fechahasta != null)
                {
                    fechahastaregistro = Convert.ToDateTime(fechahasta);
                    _ConectRegistrosCount.StParameters("fechahasta", fechahastaregistro.ToString("yyyy-MM-dd"));

                }
                else
                {
                    _ConectRegistrosCount.StParameters("fechahasta", null);
                }
                if (usuarioRegistro != null)
                {

                    _ConectRegistrosCount.StParameters("usuarioRegistro", usuarioRegistro);

                }
                else
                {
                    _ConectRegistrosCount.StParameters("usuarioRegistro", "");
                }

                if (estadoRegistro != null)
                {

                    _ConectRegistrosCount.StParameters("estadoRegistro", estadoRegistro);

                }
                else
                {
                    _ConectRegistrosCount.StParameters("estadoRegistro", "");
                }
                if (procesadoRegistro == "0")
                {

                    _ConectRegistrosCount.StParameters("procesadoRegistro", "0");

                }
                else
                {
                    _ConectRegistrosCount.StParameters("procesadoRegistro", "1");
                }
                _ConectRegistrosCount.ExecuteProcedure();

                while (_ConectRegistrosCount.DataReader.Read())
                {

                    cantidad = cantidad + 1;

                }

            }
            catch (Exception e)
            {
                _ConectRegistrosCount.Conect.Close();
                string x = e.ToString();
            }
            return cantidad;
        }


        //guarda la informacion de las actualizacion
        [HttpPost]
        [ResponseType(typeof(Titulares))]
        [Route("SetTitulares")]
        public async Task<IHttpActionResult> Post([FromBody] Titulares checkOut, string RutaBd)
        {
            DateTime fechatitular;

            if (checkOut.Cedula != "")
            {

                _Conect = new AdasysConnection();

                _Conect.FbConeccion(RutaBd);

                _Conect.StProcedure("P_TITULARESGUARDAR");

                _Conect.StParameters("idpersona", checkOut.Cedula);

                _Conect.StParameters("nombres", checkOut.Nombre1);

                _Conect.StParameters("nombre2", checkOut.Nombre2);

                _Conect.StParameters("apellidos", checkOut.Apellido1);// usada cuando se ingresa el numero de recibo

                _Conect.StParameters("apellido2", checkOut.Apellido2);

                _Conect.StParameters("telefono", checkOut.Telefono);

                _Conect.StParameters("telefamiliar", checkOut.Telefamiliar);

                _Conect.StParameters("direccion", checkOut.Direccion);

                _Conect.StParameters("departamento", checkOut.Departamento);

                _Conect.StParameters("ciudad", checkOut.Ciudad);

                _Conect.StParameters("estado", checkOut.Estado);

                _Conect.StParameters("tienemascota", checkOut.TieneMascota);

                _Conect.StParameters("seguromascota", checkOut.TieneSeguro);

                _Conect.StParameters("TIENEPLANEXEQUIAL", checkOut.TienePlanExequial);

                _Conect.StParameters("ENTIDADPLANEXEQUIAL", checkOut.EntidadPlanExequial);
                //LTotalPagado.Text = "";

                _Conect.StParameters("barrio", checkOut.Barrio);

                if (checkOut.FechaNacimiento == "")
                {
                    _Conect.StParameters("fechanacimiento", null);
                }
                else
                {
                    fechatitular = Convert.ToDateTime(checkOut.FechaNacimiento);
                    _Conect.StParameters("fechanacimiento", fechatitular.ToString("yyyy-MM-dd"));
                }

                _Conect.StParameters("email", checkOut.Email);

                _Conect.StParameters("detalle", checkOut.Detalle);

                _Conect.StParameters("usuario", checkOut.Usuario);

                _Conect.StParameters("procesado", checkOut.Procesado);

                _Conect.StParameters("POSX",checkOut.POSX);

                _Conect.StParameters("POSY", checkOut.POSY);




                try
                {
                    _Conect.ExecuteProcedureBusq();
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Informacion Almacenada Correctamente\" }]"));
                    //return Json("Informacion Almacenada Correctamente");


                }
                catch (Exception ex)
                {
                    //return Json("Error Al Almacenar");
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Error Al Almacenar "+ ex + "\" }]"));
                }



            }
            else
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Complete Los Datos Obligatorios\" }]"));
                //return Json("Complete Los Datos Obligatorios");
            }
        }

        //guarda los aliados comercial
      
        [HttpPost]
        [ResponseType(typeof(Aliados))]
        [Route("SetAliados")]
        public async Task<IHttpActionResult> Post([FromBody] Aliados checkOut, string RutaBd)
        {
          

            if (checkOut.Cedula != "")
            {

                _Conect = new AdasysConnection();

                _Conect.FbConeccion(RutaBd);

                _Conect.StProcedure("P_ACTUALIZARALIADOS");

                _Conect.StParameters("idpersona", checkOut.Cedula);

                _Conect.StParameters("nombres", checkOut.Nombre1);

                _Conect.StParameters("nombre2", checkOut.Nombre2);

                _Conect.StParameters("apellidos", checkOut.Apellido1);// usada cuando se ingresa el numero de recibo

                _Conect.StParameters("apellido2", checkOut.Apellido2);

                _Conect.StParameters("telefono", checkOut.Telefono);

                _Conect.StParameters("telefamiliar", checkOut.Telefamiliar);

                _Conect.StParameters("direccion", checkOut.Direccion);

                _Conect.StParameters("departamento", checkOut.Departamento);

                _Conect.StParameters("ciudad", checkOut.Ciudad);

                _Conect.StParameters("barrio", checkOut.Barrio);

                _Conect.StParameters("email", checkOut.Email);

                _Conect.StParameters("procesado", checkOut.Procesado);

                 _Conect.StParameters("POSX", checkOut.POSX);

                _Conect.StParameters("POSY", checkOut.POSY);

                _Conect.StParameters("categoria", checkOut.Categoria); //default participante  1. participante, 2. activo, 3.inactivo   

                //39783458
                //1. guaradar en la tabla tblactualizacionaliado como procesado 0
                //2. crear aprovacion de aliados formulario 
                //3. crear usuario automaticamente trigger
                //4. 
                //5. enviar correo de aprovacion con usuario y clave 
                //6. los aliados solo pueden ver prospecto y un nuevo formulario de consulta de solo la informacion que el registro
                //7  crear dashboard graficamente mostrando referidos acumulado y mes y me mustre que categoria es 

                //configuracion = 1 pedir cambio de contraseña
                //tbl 


                try
                {
                    _Conect.ExecuteProcedureBusq();
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Informacion Almacenada Correctamente\" }]"));
                    //return Json("Informacion Almacenada Correctamente");


                }
                catch (Exception ex)
                {
                    //return Json("Error Al Almacenar");
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Error Al Almacenar " + ex + "\" }]"));
                }



            }
            else
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Complete Los Datos Obligatorios\" }]"));
                //return Json("Complete Los Datos Obligatorios");
            }
        }




        [HttpPost]
        [ResponseType(typeof(CambioClave))]
        [Route("SetCambioClave")]
        public async Task<IHttpActionResult> SetCambioClave([FromBody] CambioClave checkOut, string rutaBd)
        {


            if (checkOut.Usuario != "")
            {

                _Conect = new AdasysConnection();

                _Conect.FbConeccion(rutaBd);

                _Conect.StProcedure("P_ACTUALIZARCAMBICLAVE");

                _Conect.StParameters("Usuario", checkOut.Usuario);

                _Conect.StParameters("Clavenueva", checkOut.Clavenueva);

                


                try
                {
                    _Conect.ExecuteProcedureBusq();
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Clave Actualizada Correctamente\" }]"));
                    //return Json("Informacion Almacenada Correctamente");


                }
                catch (Exception ex)
                {
                    //return Json("Error Al Almacenar");
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Error Al Actualizar La Clave" + ex + "\" }]"));
                }



            }
            else
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Ingrese Los Datos Obligatorios\" }]"));
                //return Json("Complete Los Datos Obligatorios");
            }
        }




        [HttpPost]
        [ResponseType(typeof(Titulares))]
        [Route("SetActualizarDatos")]
        public async Task<IHttpActionResult> SetActualizarDatos([FromBody] ConfirmarDatos checkOut, string rutaBd)
        {
            

            if (checkOut.Cedula != "")
            {

                _Conect = new AdasysConnection();

                _Conect.FbConeccion(rutaBd);

                _Conect.StProcedure("P_TITULARESACTUALIZAR");

                _Conect.StParameters("idpersona", checkOut.Cedula);

                _Conect.StParameters("usuarioregistro", checkOut.Usuario);

              

                try
                {
                    _Conect.ExecuteProcedureBusq();
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Informacion Actualizada Correctamente\" }]"));
                    //return Json("Informacion Almacenada Correctamente");


                }
                catch (Exception ex)
                {
                    //return Json("Error Al Almacenar");
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Error Al Actualizar " + ex + "\" }]"));
                }



            }
            else
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Complete Los Datos Obligatorios\" }]"));
                //return Json("Complete Los Datos Obligatorios");
            }
        }

        //trae los beneficiarios
        [HttpGet]
        [Route("GetBeneficiarios")]
        public IHttpActionResult GetBeneficiarios(string cedula, string contrato, string rutaBd)
        {
            if (cedula == "")
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            int fila = 0;
            bool encontro = false;
            DateTime fechatitular;
            _Conect = new AdasysConnection();
            _Conect.FbConeccion(rutaBd);
            _Conect.StProcedure("P_ACTUALIZACIONBENEFICIARIO"); //
            _Conect.StParameters("cedula", cedula);
            _Conect.StParameters("contrato", contrato);
            _Conect.ExecuteProcedure();

            int cantidad = cantidadRegistrosBeneficiarios(cedula, contrato,rutaBd);

            Beneficiarios[] objBeneficiarios = new Beneficiarios[cantidad];
            while (_Conect.DataReader.Read())
            {

                objBeneficiarios[fila] = new Beneficiarios();


                if (_Conect.DataReader["IDPERSONA"].ToString() != "")
                {
                    encontro = true;
                }
                else
                {
                    encontro = false;
                }



                objBeneficiarios[fila].Cedula = _Conect.DataReader["IDPERSONA"].ToString();
                objBeneficiarios[fila].Nombre1 = _Conect.DataReader["Nombres"].ToString();
                objBeneficiarios[fila].Nombre2 = _Conect.DataReader["Nombre2"].ToString();
                objBeneficiarios[fila].Apellido1 = _Conect.DataReader["Apellidos"].ToString();
                objBeneficiarios[fila].Apellido2 = _Conect.DataReader["Apellido2"].ToString();
                objBeneficiarios[fila].Direccion = _Conect.DataReader["DIRECCION"].ToString();
                objBeneficiarios[fila].Departamento = _Conect.DataReader["DEPTO"].ToString();
                objBeneficiarios[fila].Ciudad = _Conect.DataReader["Ciudad"].ToString();
                objBeneficiarios[fila].Barrio = _Conect.DataReader["BOXCONTRA"].ToString();
                objBeneficiarios[fila].Telefono = _Conect.DataReader["Telefono"].ToString();
                objBeneficiarios[fila].Telefamiliar = _Conect.DataReader["CELULAR"].ToString();
                objBeneficiarios[fila].Email = _Conect.DataReader["PARENTES"].ToString();
                objBeneficiarios[fila].Tipo = _Conect.DataReader["Tipo"].ToString();
                string fnacimiento = _Conect.DataReader["FNACIMIENTO"].ToString();
                if (fnacimiento != "")
                {
                    fechatitular = Convert.ToDateTime(fnacimiento);
                    objBeneficiarios[fila].FechaNacimiento = fechatitular.ToString("yyyy-MM-dd");
                }
                objBeneficiarios[fila].Estado = _Conect.DataReader["Estado"].ToString();

                fila = fila + 1;
            }
           
            if (encontro == true)
            {
                return Json(objBeneficiarios);
            }
            else
            {

                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Sin Datos\" }]"));

            }
            // return Json(objTitulares);
        }

        private int cantidadRegistrosBeneficiarios(string cedula, string contrato, string rutaBd)
        {
            int cantidad = 0;
            if (cedula == "")
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            try
            {
               
                _ConectBeneficiarios = new AdasysConnection();
                _ConectBeneficiarios.FbConeccion(rutaBd);
                _ConectBeneficiarios.StProcedure("P_ACTUALIZACIONBENEFICIARIO"); //
                _ConectBeneficiarios.StParameters("cedula", cedula);
                _ConectBeneficiarios.StParameters("contrato", contrato);
                _ConectBeneficiarios.ExecuteProcedure();


                while (_ConectBeneficiarios.DataReader.Read())
                {
                    cantidad = cantidad + 1;

                }
            }
            catch (Exception e)
            {
                _ConectBeneficiarios.Conect.Close();
                string x = e.ToString();
            }
            return cantidad;


        }


        //actualiza los beneficiarios
        //guarda la informacion de los beneficiarios
        [HttpPost]
        [ResponseType(typeof(Beneficiarios))]
        [Route("SetBeneficiarios")]
        public async Task<IHttpActionResult> Post([FromBody] Beneficiarios checkOut, string rutaBd)
        {
            DateTime fechatitular;

            if (checkOut.Cedula != "")
            {

                _Conect = new AdasysConnection();

                _Conect.FbConeccion(rutaBd);

                _Conect.StProcedure("P_BENEFICIARIOSGUARDAR"); //P_TITULARESGUARDAR

                _Conect.StParameters("idpersona", checkOut.Cedula);

                _Conect.StParameters("nombres", checkOut.Nombre1);

                _Conect.StParameters("nombre2", checkOut.Nombre2);

                _Conect.StParameters("apellidos", checkOut.Apellido1);// usada cuando se ingresa el numero de recibo

                _Conect.StParameters("apellido2", checkOut.Apellido2);

                _Conect.StParameters("telefono", checkOut.Telefono);

                _Conect.StParameters("telefamiliar", checkOut.Telefamiliar);

                _Conect.StParameters("direccion", checkOut.Direccion);

                _Conect.StParameters("departamento", checkOut.Departamento);

                _Conect.StParameters("ciudad", checkOut.Ciudad);

                _Conect.StParameters("estado", checkOut.Estado);

                _Conect.StParameters("tienemascota", checkOut.TieneMascota);

                _Conect.StParameters("seguromascota", checkOut.TieneSeguro);
                //LTotalPagado.Text = "";

                _Conect.StParameters("barrio", checkOut.Barrio);

                if (checkOut.FechaNacimiento == "")
                {
                    _Conect.StParameters("fechanacimiento", null);
                }
                else
                {
                    fechatitular = Convert.ToDateTime(checkOut.FechaNacimiento);
                    _Conect.StParameters("fechanacimiento", fechatitular.ToString("yyyy-MM-dd"));
                }

                _Conect.StParameters("email", checkOut.Email);

                _Conect.StParameters("detalle", checkOut.Detalle);

                _Conect.StParameters("usuario", checkOut.Usuario);

                _Conect.StParameters("procesado", checkOut.Procesado);

                _Conect.StParameters("POSX", checkOut.POSX);

                _Conect.StParameters("POSY", checkOut.POSY);




                try
                {
                    _Conect.ExecuteProcedureBusq();
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Informacion Almacenada Correctamente\" }]"));
                    //return Json("Informacion Almacenada Correctamente");


                }
                catch (Exception ex)
                {
                    //return Json("Error Al Almacenar");
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Error Al Almacenar " + ex + "\" }]"));
                }



            }
            else
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Complete Los Datos Obligatorios\" }]"));
                //return Json("Complete Los Datos Obligatorios");
            }
        }



        [HttpPost]
        [ResponseType(typeof(Titulares))]
        [Route("SetEnviarSms")]
        public async Task<IHttpActionResult> SetEnviarSms([FromBody] EnviarSms checkOut)
        {


            if (checkOut.Celular != "" && checkOut.Mensaje != "")
            {
                
                try
                {

                    //envia el mensaje si el pago se realizo   
                    string estado = "";
                    SigmaSMS smsEnvioPisco = new SigmaSMS();
                    
                    estado = smsEnvioPisco.EnviarSMS(checkOut.Celular, "57;" + checkOut.Celular + "; " + checkOut.Mensaje, checkOut.Apikey, checkOut.Apisecret);

                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Mensaje Enviado Correctamente\" }]"));
                    //return Json("Informacion Almacenada Correctamente");

                }
                catch (Exception ex)
                {
                    //return Json("Error Al Almacenar");
                    JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                    return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Error Al Actualizar " + ex + "\" }]"));
                }



            }
            else
            {
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                return Json(json_serializer.DeserializeObject("[{ \"Estado\":\"Complete Los Datos Obligatorios\" }]"));
                //return Json("Complete Los Datos Obligatorios");
            }
        }
    }
}
