using L01_NUMEROS_CARNETS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace L01_NUMEROS_CARNETS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ComentariosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListaComentarios")]
        public IActionResult ListaComentarios()
        {
            var comentarios = (from c in _context.Comentarios
                               select c).ToList();
            return Ok(comentarios);
        }


        [HttpPost]
        [Route("CrearComentario")]
        public IActionResult CrearComentario([FromBody] Comentario comentario)
        {
            //el id no es necesario
            //lo hicismo asi ya que nos daba error 
            /*
             SqlException: No se puede insertar un valor explícito en la columna de identidad de la tabla 'comentarios' cuando IDENTITY_INSERT es OFF.
            */
            // y no queriamos modificar la base de datos ni las propiedades de ellas
            //por esa manera lo hicimos de la siguiente manera
            _context.Comentarios.Add(new Comentario
            {
                PublicacionId = comentario.PublicacionId,
                Comentario1 = comentario.Comentario1,
                UsuarioId = comentario.UsuarioId
            });

            _context.SaveChanges();
            return Ok(comentario);
        }


        [HttpPut]
        [Route("UpdateComentario/{id}")]
        public IActionResult UpdateComentario(int id, [FromBody] Comentario comentarioActualizado)
        {
            var comentario = (from c in _context.Comentarios
                              where c.CometarioId == id
                              select c).FirstOrDefault();
            if (comentario == null) return NotFound();

            comentario.PublicacionId = comentarioActualizado.PublicacionId ?? comentario.PublicacionId;
            comentario.Comentario1 = comentarioActualizado.Comentario1 ?? comentario.Comentario1;
            comentario.UsuarioId = comentarioActualizado.UsuarioId ?? comentario.UsuarioId;

            _context.SaveChanges();
            return Ok(comentario);
        }

        [HttpDelete]
        [Route("DeleteComentario/{id}")]
        public IActionResult DeleteComentario(int id)
        {
            var comentario = (from c in _context.Comentarios
                              where c.CometarioId == id
                              select c).FirstOrDefault();
            if (comentario == null) return NotFound();

            _context.Comentarios.Remove(comentario);
            _context.SaveChanges();
            return Ok(200);
        }







        [HttpGet]
        [Route("GetComentariosPorUsuario/{usuarioId}")]
        public IActionResult GetComentariosPorUsuario(int usuarioId)
        {
            var comentarios = (from c in _context.Comentarios
                               where c.UsuarioId == usuarioId
                               select c).ToList();
            return Ok(comentarios);
        }
    }
}
