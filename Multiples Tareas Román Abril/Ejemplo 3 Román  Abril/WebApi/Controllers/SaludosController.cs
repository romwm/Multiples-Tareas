using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("Saludos")]
    [ApiController]
    public class SaludosController:ControllerBase
    {
        [HttpGet("{nombre}")]
        public ActionResult<string> ObtenerSaludo(string nombre)
        {
            return $"Saludos, {nombre}!";
        }
    }
    

}
