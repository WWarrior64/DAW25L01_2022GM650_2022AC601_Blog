using L01_2022GM650_2022AC601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2022GM650_2022AC601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class publicacionesController : ControllerBase
    {
        private readonly blogDBContext _blogDBContexto;

        public publicacionesController(blogDBContext blogDBContexto)
        {
            _blogDBContexto = blogDBContexto;
        }

        /*Obtener publicaciones*/
        [HttpGet]
        [Route("GetAllPublicaciones")]
        public IActionResult GetPublicaciones()
        {
            var listadoPublicaciones = (from e in _blogDBContexto.publicaciones select e).ToList();

            if (listadoPublicaciones.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoPublicaciones);
        }


        /*Guardar un nuevo registro*/
        [HttpPost]
        [Route("AddPublicacion")]
        public IActionResult GuardarPublicacion([FromBody] publicaciones publicaciones)
        {

            try
            {
                _blogDBContexto.Add(publicaciones);
                _blogDBContexto.SaveChanges();
                return Ok(publicaciones);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        /*Actualizar los datos*/
        [HttpPut]
        [Route("actualizar/{id}")]
        public IActionResult ActualizarPublicacion(int id, [FromBody] publicaciones publicacionModificar)
        {


            publicaciones? publicacionActual = (from e in _blogDBContexto.publicaciones
                                                where e.publicacionId == id
                                                select e).FirstOrDefault();


            if (publicacionActual == null)
            { return NotFound(); }


            publicacionActual.titulo = publicacionModificar.titulo;
            publicacionActual.descripcion = publicacionModificar.descripcion;
            publicacionActual.usuarioId = publicacionModificar.usuarioId;


            _blogDBContexto.Entry(publicacionActual).State = EntityState.Modified;
            _blogDBContexto.SaveChanges();

            return Ok(publicacionModificar);

        }

        /*Eliminar los datos*/

        [HttpDelete]
        [Route("eliminar/{id}")]

        public IActionResult ElimiarPublicacion(int id)
        {

            publicaciones? publicaciones = (from e in _blogDBContexto.publicaciones
                                            where e.publicacionId == id
                                            select e).FirstOrDefault();

            if (publicaciones == null)
                return NotFound();

            _blogDBContexto.publicaciones.Attach(publicaciones);
            _blogDBContexto.publicaciones.Remove(publicaciones);
            _blogDBContexto.SaveChanges();

            return Ok(publicaciones);

        }

        /*Retorno por usuario en especifico*/
        /*Se incluyo tanto por Id como por Nombre*/
        /// <param name="usuarioId"></param> 
        [HttpGet]
        [Route("GetByUsuario/{usuarioId}")]
        public IActionResult GetporUsuario(int usuarioId)
        {
            var publicacionesUsuario = (from p in _blogDBContexto.publicaciones
                                        where p.usuarioId == usuarioId
                                        select new
                                        {
                                            p.publicacionId,
                                            p.titulo,
                                            p.descripcion,
                                            p.usuarioId
                                        }).ToList();


            if (publicacionesUsuario == null || publicacionesUsuario.Count == 0)
            {
                return NotFound();
            }

            return Ok(publicacionesUsuario);
        }


        /// <param name="nombreUsuario"></param> 
        [HttpGet]
        [Route("GetByNombreUsuario/{nombreUsuario}")]
        public IActionResult GetporNombre(string nombreUsuario)
        {

            var usuario = _blogDBContexto.usuarios
                .FirstOrDefault(u => u.nombreUsuario == nombreUsuario);


            if (usuario == null)
            {
                return NotFound();
            }


            var publicacionesUsuario = (from p in _blogDBContexto.publicaciones
                                        where p.usuarioId == usuario.usuarioId
                                        select new
                                        {
                                            p.publicacionId,
                                            p.titulo,
                                            p.descripcion,
                                            p.usuarioId
                                        }).ToList();


            if (publicacionesUsuario == null || publicacionesUsuario.Count == 0)
            {
                return NotFound();
            }

            return Ok(publicacionesUsuario);
        }


    }
}
