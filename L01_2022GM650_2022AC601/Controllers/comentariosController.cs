using L01_2022GM650_2022AC601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2022GM650_2022AC601.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class comentariosController : ControllerBase
	{
		private readonly blogDBContext _blogDBContexto;

		public comentariosController(blogDBContext blogDBContexto)
		{
			_blogDBContexto = blogDBContexto;
		}

		/// <summary>
		/// EndPoint que retorna el listado de todos los registros existentes
		/// </summary>
		[HttpGet]
		[Route("GetAllcomentarios")]
		public IActionResult GetComentarios()
		{
			var listadoComentarios = (from e in _blogDBContexto.comentarios
								   join t in _blogDBContexto.publicaciones on e.publicacionId equals t.publicacionId
								   join q in _blogDBContexto.usuarios on e.usuarioId equals q.usuarioId
								   select new
								   {
									   e.comentarioId,
									   e.publicacionId,
									   Titulo_publicación = t.titulo,
									   e.comentario,
									   e.usuarioId,
									   nombre_usuario = q.nombreUsuario,
									   nombre = q.nombre,
									   apellido = q.apellido

								   }).ToList();


			if (listadoComentarios.Count == 0)
			{
				return NotFound();
			}

			return Ok(listadoComentarios);
		}

		/// <summary> 
		/// EndPoint que retorna los registros de una tabla filtrados por publicacion
		/// </summary> 
		/// <returns></returns> 
		[HttpGet]
		[Route("FilterByPublicacion/{id}")]

		public IActionResult FilterComentarioPorPublicacion(int id)
		{
			var comentarioLista = (from e in _blogDBContexto.comentarios
								join t in _blogDBContexto.publicaciones on e.publicacionId equals t.publicacionId
								join q in _blogDBContexto.usuarios on e.usuarioId equals q.usuarioId
								where e.publicacionId == id
								select new
								{
									e.comentarioId,
									e.publicacionId,
									Titulo_publicación = t.titulo,
									e.comentario,
									e.usuarioId,
									nombre_usuario = q.nombreUsuario,
									nombre = q.nombre,
									apellido = q.apellido
								}).ToList();

			if (comentarioLista.Count == 0)
			{
				return NotFound();
			}

			return Ok(comentarioLista);
		}

		[HttpPost]
		[Route("Add")]
		public ActionResult GuardarComentario([FromBody] comentarios comentario)
		{
			try
			{
				_blogDBContexto.comentarios.Add(comentario);
				_blogDBContexto.SaveChanges();
				return Ok(comentario);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut]
		[Route("actualizar/{id}")]
		public IActionResult ActualizarComentarios(int id, [FromBody] comentarios comentarioModificar)
		{
			
			comentarios? comentarioActual = (from e in _blogDBContexto.comentarios where e.comentarioId == id select e).FirstOrDefault();

			
			if (comentarioActual == null)
			{
				return NotFound();
			}

		
			comentarioActual.publicacionId = comentarioModificar.publicacionId;
            comentarioActual.comentario = comentarioModificar.comentario;
            comentarioActual.usuarioId = comentarioModificar.usuarioId;

         
            _blogDBContexto.Entry(comentarioActual).State = EntityState.Modified;
			_blogDBContexto.SaveChanges();
			return Ok(comentarioModificar);
		}

		[HttpDelete]
		[Route("eliminar/{id}")]
		public IActionResult EliminarComentario(int id)
		{
			
			comentarios? comentario = (from e in _blogDBContexto.comentarios where e.comentarioId == id select e).FirstOrDefault();

			if (comentario == null)
				return NotFound();
 
			_blogDBContexto.comentarios.Attach(comentario);
			_blogDBContexto.comentarios.Remove(comentario);
			_blogDBContexto.SaveChanges();
			return Ok(comentario);
		}
	}
}
