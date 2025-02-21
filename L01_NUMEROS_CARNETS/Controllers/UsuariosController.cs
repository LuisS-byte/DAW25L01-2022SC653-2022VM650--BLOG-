using L01_NUMEROS_CARNETS.Models;
using Microsoft.AspNetCore.Mvc;

namespace L01_NUMEROS_CARNETS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListaUsuario")]
        public IActionResult ListaUsuario()
        {
            var usuarios = (from u in _context.Usuarios
                            select u).ToList();
            return Ok(usuarios);
        }

        [HttpPost]
        [Route("CrearUsuario")]
        public IActionResult CrearUsuario([FromBody] Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return Ok(usuario);
        }

        [HttpPut]
        [Route("ActualizarUsuario/{id}")]
        public IActionResult ActualizarUsuario(int id, [FromBody] Usuario usuarioActualizado)
        {
            var dataDB = (from u in _context.Usuarios
                          where u.UsuarioId == id
                          select u).FirstOrDefault();
            if (dataDB == null) return NotFound();

            dataDB.NombreUsuario = usuarioActualizado.NombreUsuario ?? dataDB.NombreUsuario;
            dataDB.Clave = usuarioActualizado.Clave ?? dataDB.Clave;
            dataDB.Nombre = usuarioActualizado.Nombre ?? dataDB.Nombre;
            dataDB.Apellido = usuarioActualizado.Apellido ?? dataDB.Apellido;
            dataDB.RolId = usuarioActualizado.RolId ?? dataDB.RolId;

            _context.SaveChanges();
            return Ok(dataDB);
        }

        [HttpDelete]
        [Route("EliminarUsaurio/{id}")]
        public IActionResult EliminarUsaurio(int id)
        {
            var dataDB = (from u in _context.Usuarios
                          where u.UsuarioId == id
                          select u).FirstOrDefault();
            if (dataDB == null) return NotFound();

            _context.Usuarios.Remove(dataDB);
            _context.SaveChanges();
            return Ok(200);
        }

        [HttpGet]
        [Route("GetUsuariosPorRol/{rolId}")]
        public IActionResult GetUsuariosPorRol(int rolId)
        {
            var dataDB = (from u in _context.Usuarios
                          where u.RolId == rolId
                          select u).ToList();
            return Ok(dataDB);
        }

        [HttpGet]
        [Route("GetUsuariosPorNombreApellido/{nombre}/{apellido}")]
        public IActionResult GetUsuariosPorNombreApellido(string nombre, string apellido)
        {
            var dataDB = (from u in _context.Usuarios
                          where u.Nombre.Contains(nombre) && u.Apellido.Contains(apellido)
                          select u).ToList();
            return Ok(dataDB);
        }

        [HttpGet]
        [Route("TopUsuarios/{n}")]
        public IActionResult GetTopUsuarios(int n)
        {
            var topUsuarios = (from u in _context.Usuarios
                               join c in _context.Comentarios on u.UsuarioId equals c.UsuarioId
                               group c by new { u.UsuarioId, u.Nombre, u.Apellido } into grouped
                               orderby grouped.Count() descending
                               select new
                               {
                                   UsuarioId = grouped.Key.UsuarioId,
                                   Nombre = grouped.Key.Nombre,
                                   Apellido = grouped.Key.Apellido,
                                   CantidadComentarios = grouped.Count()
                               }).Take(n).ToList();

            if (topUsuarios.Count == 0)
            {
                return NotFound();
            }

            return Ok(topUsuarios);
        }
    }
}
