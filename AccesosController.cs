using API_SAADS_CORE_61.DTOs;
using API_SAADS_CORE_61.Helpers;
using API_SAADS_CORE_61.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using System.Drawing;

namespace API_SAADS_CORE_61.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccesosController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IRepositorio _repsositorio;
        private const int moduloId = 1300;// colocar el modulo correspodiente

    
        public AccesosController(IRepositorio repositorio, IWebHostEnvironment env)
        {
            _env = env;
            _repsositorio = repositorio;
        }

        //var respuestaHTTP = await _repsositorio.Post<PagoGenerarDTO, PagoGenerarResp>("RegistrarPago", consulta);
        //var res = respuestaHTTP.Response;


        [HttpGet("RenovarToken")]
        [Authorize(Roles = "funcionario")]
        public async Task<ActionResult<UserToken>> RenovarToken()
        {
            try
            {
                var respuestaHTTP = await _repsositorio.Get<UserToken>("Funcionarios/RenovarToken");
                var res = respuestaHTTP.Response;

                if (respuestaHTTP.Error)
                {
                    return BadRequest(respuestaHTTP.HttpResponseMessage.ToString());
                }

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("Funcionario")]
        [Authorize(Roles = "funcionario")]
        public async Task<ActionResult<FuncionarioDTO>> Get()
        {

            try
            {
                var respuestaHTTP = await _repsositorio.Get<FuncionarioDTO>("Funcionarios");
                var res = respuestaHTTP.Response;

                if (respuestaHTTP.Error)
                {
                    return BadRequest(respuestaHTTP.HttpResponseMessage.ToString());
                }

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }




        }

        [HttpGet("Tareas/{interfaceId:int}")]
        [Authorize(Roles = "funcionario")]
        public async Task<ActionResult<List<TareaDTO>>> GetTareas(int interfaceId)
        {

            try
            {
                var respuestaHTTP = await _repsositorio.Get<TareaDTO>($"Funcionarios/Tareas/{interfaceId}");
                var res = respuestaHTTP.Response;

                if (respuestaHTTP.Error)
                {
                    return BadRequest(respuestaHTTP.HttpResponseMessage.ToString());
                }

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        [HttpGet("InterfaceTareas")]
        [Authorize(Roles = "funcionario")]
        public async Task<ActionResult<List<InterfaceDTO>>> GetInterfaceTareas()
        {
            try
            {
                var respuestaHTTP = await _repsositorio.Get<List<InterfaceDTO>>($"Funcionarios/InterfaceTareas/{moduloId}");
                var res = respuestaHTTP.Response;

                if (respuestaHTTP.Error)
                {
                    return BadRequest(respuestaHTTP.HttpResponseMessage.ToString());
                }

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }
        [HttpGet("QR")]
        [AllowAnonymous]
        public async Task<ActionResult<byte[]>> getQR(string texto)
        {
            try
            {
                return QR.GenerarQR(texto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }

        //Verbo de accion (HttPost) de tipo POST
        //Renombrar la ruta de la accion a "cargar-archivo" con el atributo "Route"
        [HttpPost, Route("cargar-archivo")]
        public async Task<ActionResult<byte[]>> UploadFile(string texto)
        {
            //Variable que retorna el valor del resultado del metodo
            //El valor predeterminado es Falso (false)
            string resultado = "";

            //La variable "file" recibe el archivo en el objeto Request.Form
            //Del POST que realiza la aplicacion a este servicio.
            //Se envia un formulario completo donde uno de los valores es el archivo
            var file = Request.Form.Files[0];

            //Variable donde se coloca la ruta relativa de la carpeta de destino
            //del archivo cargado
            string NombreCarpeta = "Archivos\\";

            //Variable donde se coloca la ruta raíz de la aplicacion
            //para esto se emplea la variable "_env" antes de declarada
            string RutaRaiz = _env.ContentRootPath;

            //Se concatena las variables "RutaRaiz" y "NombreCarpeta"
            //en una otra variable "RutaCompleta"
            string RutaCompleta = RutaRaiz + NombreCarpeta;


            //Se valida con la variable "RutaCompleta" si existe dicha carpeta            
            if (!Directory.Exists(RutaCompleta))
            {
                //En caso de no existir se crea esa carpeta
                Directory.CreateDirectory(RutaCompleta);
            }

            //Se valida si la variable "file" tiene algun archivo
            if (file.Length > 0)
            {
                //Se declara en esta variable el nombre del archivo cargado
                string NombreArchivo = "LOGOP.jpg";

                //Se declara en esta variable la ruta completa con el nombre del archivo
                string RutaFullCompleta = Path.Combine(RutaCompleta, NombreArchivo);

                //Se crea una variable FileStream para carlo en la ruta definida
                using (var stream = new FileStream(RutaFullCompleta, FileMode.Truncate))
                {
                    file.CopyTo(stream);

                    //Como se cargo correctamente el archivo
                    //la variable "resultado" llena el valor "true"
                    resultado = NombreArchivo;
                    
                }

            }
            string rutaDelete = RutaCompleta + resultado;
            //Se retorna la variable "resultado" como resultado de una tarea
             return QR.GenerarQRWithUploadImage(texto,resultado, rutaDelete);
           //return Path.Combine(RutaCompleta+resultado);

        }

        [HttpPost("imagen")]
        [AllowAnonymous]
        public async Task<ActionResult<byte[]>> getImg(byte[] imagen)
        {
            try
            {
                return imagen;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }


        [HttpGet("Interfaces")]
        [Authorize(Roles = "funcionario")]
        public async Task<ActionResult<List<InterfaceDTO>>> getIterfaces()
        {

            try
            {
                var respuestaHTTP = await _repsositorio.Get<List<InterfaceDTO>>($"Funcionarios/Interfaces/{moduloId}");
                var res = respuestaHTTP.Response;

                if (respuestaHTTP.Error)
                {
                    return BadRequest(respuestaHTTP.HttpResponseMessage.ToString());
                }

                return Ok(res);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }
    }
}
