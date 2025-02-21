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
		/// EndPoint que retorna el listado de todos los equipos existentes
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
									   nombre_usuario = q.nombreUsuario

								   }).ToList();


			if (listadoComentarios.Count == 0)
			{
				return NotFound();
			}

			return Ok(listadoComentarios);
		}

		/// <summary> 
		/// EndPoint que retorna los registros de una tabla filtrados por descripcion 
		/// </summary> 
		/// <returns></returns> 
		[HttpGet]
		[Route("FilterByPublicacion/{publicacionId}")]

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
									nombre_usuario = q.nombreUsuario
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
		public IActionResult ActualizarUsuario(int id, [FromBody] usuarios usuarioModificar)
		{
			//Para actualizar un registro, se obtiene el registro original de la base de datos 
			//al cual alteraremos alguna propiedad 
			usuarios? usuarioActual = (from e in _blogDBContexto.usuarios where e.usuarioId == id select e).FirstOrDefault();

			//Verificamos que exista el registro segun su ID 
			if (usuarioActual == null)
			{
				return NotFound();
			}

			//Si se encuentra el registro, se alteran los campos modificables 
			usuarioActual.rolId = usuarioModificar.rolId;
			usuarioActual.nombreUsuario = usuarioModificar.nombreUsuario;
			usuarioActual.clave = usuarioModificar.clave;
			usuarioActual.nombre = usuarioModificar.nombre;
			usuarioActual.apellido = usuarioModificar.apellido;
			//Se marca el registro como modificado en el contexto 
			//y se envia la modificacion a la base de datos 
			_blogDBContexto.Entry(usuarioActual).State = EntityState.Modified;
			_blogDBContexto.SaveChanges();
			return Ok(usuarioModificar);
		}

		[HttpDelete]
		[Route("eliminar/{id}")]
		public IActionResult EliminarUsuario(int id)
		{
			//Para actualizar un registro, se obtiene el registro original de la base de datos 
			//al cual eliminaremos 
			usuarios? usuario = (from e in _blogDBContexto.usuarios where e.usuarioId == id select e).FirstOrDefault();

			//Verificamos que exista el registro segun su ID 
			if (usuario == null)
				return NotFound();

			//Ejecutamos la accion de elminar el registro 
			_blogDBContexto.usuarios.Attach(usuario);
			_blogDBContexto.usuarios.Remove(usuario);
			_blogDBContexto.SaveChanges();
			return Ok(usuario);
		}
	}
}
