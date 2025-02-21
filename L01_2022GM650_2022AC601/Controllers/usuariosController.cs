using L01_2022GM650_2022AC601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2022GM650_2022AC601.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class usuariosController : ControllerBase
	{
		private readonly blogDBContext _blogDBContexto;

		public usuariosController(blogDBContext blogDBContexto)
		{
			_blogDBContexto = blogDBContexto;
		}

		/// <summary>
		/// EndPoint que retorna el listado de todos los equipos existentes
		/// </summary>
		[HttpGet]
		[Route("GetAllusuarios")]
		public IActionResult GetUsuarios()
		{
			var listadoUsuarios = (from e in _blogDBContexto.usuarios join t in _blogDBContexto.roles on e.rolId equals t.rolId
											  select new
											  {
												  e.usuarioId,
												  e.rolId,
												  Rol = t.rol,
												  e.nombreUsuario,
												  e.clave,
												  e.nombre,
												  e.apellido,
											  }).ToList();


			if (listadoUsuarios.Count == 0)
			{
				return NotFound();
			}

			return Ok(listadoUsuarios);
		}

		/// <summary> 
		/// EndPoint que retorna los registros de una tabla filtrados por su nombre y apellido
		/// </summary> 
		[HttpGet]
		[Route("FilterByNombreApellido/{nombre}/{apellido}")]
		public IActionResult FilterUsuarioPorNombreApellido(string nombre, string apellido)
		{
			var usuarioLista = (from e in _blogDBContexto.usuarios
								 join t in _blogDBContexto.roles on e.rolId equals t.rolId
								 where e.nombre.Contains(nombre) && e.apellido.Contains(apellido)
								 select new
								 {
									 e.usuarioId,
									 e.rolId,
									 Rol = t.rol,
									 e.nombreUsuario,
									 e.clave,
									 e.nombre,
									 e.apellido,
								 }).ToList();

			if (usuarioLista.Count == 0)
			{
				return NotFound();
			}

			return Ok(usuarioLista);
		}

		/// <summary> 
		/// EndPoint que retorna los registros de una tabla filtrados por descripcion 
		/// </summary> 
		/// <returns></returns> 
		[HttpGet]
		[Route("FilterByRol/{id}")]

		public IActionResult FilterUsuarioPorRol(int id)
		{
			var usuarioLista = (from e in _blogDBContexto.usuarios
									  join t in _blogDBContexto.roles on e.rolId equals t.rolId
									  where e.rolId == id
									  select new
									  {
										  e.usuarioId,
										  e.rolId,
										  Rol = t.rol,
										  e.nombreUsuario,
										  e.clave,
										  e.nombre,
										  e.apellido,
									  }).ToList();

			if (usuarioLista.Count == 0)
			{
				return NotFound();
			}

			return Ok(usuarioLista);
		}

		[HttpPost]
		[Route("Add")]
		public ActionResult GuardarUsuario([FromBody] usuarios usuario)
		{
			try
			{
				_blogDBContexto.usuarios.Add(usuario);
				_blogDBContexto.SaveChanges();
				return Ok(usuario);
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
