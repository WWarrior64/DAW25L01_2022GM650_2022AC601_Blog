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
            var listadoPublicaciones = (from e in _blogDBContexto.publicaciones
										join u in _blogDBContexto.usuarios on
                                        e.usuarioId equals u.usuarioId
										select new
										{
											e.publicacionId,
											e.titulo,
											e.descripcion,
											e.usuarioId,
											nombre_Usuario = u.nombreUsuario,
											nombre = u.nombre,
											apellido = u.apellido
										}).ToList();

            if (listadoPublicaciones.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoPublicaciones);
        }


		/*Retorno por usuario en especifico*/
		/*Se incluyo tanto por Id como por Nombre*/
		/// <param name="usuarioId"></param> 
		[HttpGet]
		[Route("GetByUsuario/{usuarioId}")]
		public IActionResult GetporUsuario(int usuarioId)
		{
			var publicacionesUsuario = (from p in _blogDBContexto.publicaciones
										join u in _blogDBContexto.usuarios on
										p.usuarioId equals u.usuarioId
										where p.usuarioId == usuarioId
										select new
										{
											p.publicacionId,
											p.titulo,
											p.descripcion,
											p.usuarioId,
											nombre_Usuario = u.nombreUsuario,
											nombre = u.nombre,
											apellido = u.apellido
										}).ToList();


			if (publicacionesUsuario.Count == 0)
			{
				return NotFound();
			}

			return Ok(publicacionesUsuario);
		}


		/// <param name="nombreUsuario"></param> 
		[HttpGet]
		[Route("GetByNombreDelUsuario/{nombre}")]
		public IActionResult GetporNombre(string nombre)
		{


			var publicacionesUsuario = (from p in _blogDBContexto.publicaciones
										join u in _blogDBContexto.usuarios on
										p.usuarioId equals u.usuarioId
										where u.nombre.Contains(nombre)
										select new
										{
											p.publicacionId,
											p.titulo,
											p.descripcion,
											p.usuarioId,
											nombre_Usuario = u.nombreUsuario,
											nombre = u.nombre,
											apellido = u.apellido
										}).ToList();


			if (publicacionesUsuario.Count == 0)
			{
				return NotFound();
			}

			return Ok(publicacionesUsuario);
		}

		/*Obtener publicaciones*/
		/// EndPoint que tiene el top N de comentarios junto con su cantidad de comentarios
		[HttpGet]
		[Route("GetTopNComentarios/{topN}")]
		public IActionResult GetTopNComentarios(int topN)
		{
			var listadoPublicaciones = (from e in _blogDBContexto.publicaciones
										join u in _blogDBContexto.usuarios on
										e.usuarioId equals u.usuarioId
										join c in _blogDBContexto.comentarios on 
										e.publicacionId equals c.publicacionId into comentarios
										select new
										{
											e.publicacionId,
											e.titulo,
											e.descripcion,
											nombre_Usuario = u.nombreUsuario,
											nombre = u.nombre,
											apellido = u.apellido,
											cantidad_comentarios = comentarios.Count()
										}).OrderByDescending(x => x.cantidad_comentarios)
										.Take(topN).ToList();

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

    }
}
