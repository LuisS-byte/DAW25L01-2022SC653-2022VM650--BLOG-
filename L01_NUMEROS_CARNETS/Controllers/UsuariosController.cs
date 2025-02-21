using L01_NUMEROS_CARNETS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        [Route("GetAllUsuarios")]
        public IActionResult GetAllUsuarios()
        {
            var usuarios = _context.Usuarios.ToList();
            return Ok(usuarios);
        }

        [HttpPost]
        [Route("CreateUsuario")]
        public IActionResult CreateUsuario([FromBody] Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return Ok(usuario);
        }

        

        [HttpPut]
        [Route("UpdateUsuario/{id}")]
        public IActionResult UpdateUsuario(int id, [FromBody] Usuario usuarioActualizado)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == id);
            if (usuario == null) return NotFound();

            usuario.NombreUsuario = usuarioActualizado.NombreUsuario ?? usuario.NombreUsuario;
            usuario.Clave = usuarioActualizado.Clave ?? usuario.Clave;
            usuario.Nombre = usuarioActualizado.Nombre ?? usuario.Nombre;
            usuario.Apellido = usuarioActualizado.Apellido ?? usuario.Apellido;
            usuario.RolId = usuarioActualizado.RolId ?? usuario.RolId;

            _context.SaveChanges();
            return Ok(usuario);
        }

        [HttpDelete]
        [Route("DeleteUsuario/{id}")]
        public IActionResult DeleteUsuario(int id)
        {
            var usuario = _context.Usuarios.FirstOrDefault(u => u.UsuarioId == id);
            if (usuario == null) return NotFound();

            _context.Usuarios.Remove(usuario);
            _context.SaveChanges();
            return Ok(200);
        }

        

        [HttpGet]
        [Route("GetUsuariosPorRol/{rolId}")]
        public IActionResult GetUsuariosPorRol(int rolId)
        {
            var usuarios = _context.Usuarios
                                   .Where(u => u.RolId == rolId)
                                   .ToList();
            return Ok(usuarios);
        }




        [HttpGet]
        [Route("GetUsuariosPorNombreApellido/{nombre}/{apellido}")]
        public IActionResult GetUsuariosPorNombreApellido(string nombre, string apellido)
        {
            var usuarios = _context.Usuarios
                                   .Where(u => u.Nombre.Contains(nombre) && u.Apellido.Contains(apellido))
                                   .ToList();
            return Ok(usuarios);
        }
    }
}
