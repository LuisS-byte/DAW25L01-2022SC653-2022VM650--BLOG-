using L01_NUMEROS_CARNETS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> ListaComentarios()
        {
            var dataDB = await (from c in _context.Comentarios select c).ToListAsync();
            return Ok(dataDB);
        }

        [HttpPost]
        [Route("CrearComentario")]
        public async Task<IActionResult> CrearComentario([FromBody] Comentario comentario)
        {
            //el id no es necesario
            //lo hicimos asi ya que nos daba error 
            /*
             SqlException: No se puede insertar un valor explícito en la columna de identidad de la tabla 'comentarios' cuando IDENTITY_INSERT es OFF.
            */
            // y no queriamos modificar la base de datos ni las propiedades de ellas
            //por esa manera lo hicimos de la siguiente manera
            await _context.Comentarios.AddAsync(new Comentario
            {
                PublicacionId = comentario.PublicacionId,
                Comentario1 = comentario.Comentario1,
                UsuarioId = comentario.UsuarioId
            });

            await _context.SaveChangesAsync();
            return Ok(comentario);
        }

        [HttpPut]
        [Route("ActualizarComentario/{id}")]
        public async Task<IActionResult> ActualizarComentario(int id, [FromBody] Comentario comentarioActualizado)
        {
            var dataDB = await (from c in _context.Comentarios where c.CometarioId == id select c).FirstOrDefaultAsync();
            if (dataDB == null) return NotFound();

            dataDB.PublicacionId = comentarioActualizado.PublicacionId ?? dataDB.PublicacionId;
            dataDB.Comentario1 = comentarioActualizado.Comentario1 ?? dataDB.Comentario1;
            dataDB.UsuarioId = comentarioActualizado.UsuarioId ?? dataDB.UsuarioId;

            await _context.SaveChangesAsync();
            return Ok(dataDB);
        }

        [HttpDelete]
        [Route("DeleteComentario/{id}")]
        public async Task<IActionResult> DeleteComentario(int id)
        {
            var dataDB = await (from c in _context.Comentarios where c.CometarioId == id select c).FirstOrDefaultAsync();
            if (dataDB == null) return NotFound();

            _context.Comentarios.Remove(dataDB);
            await _context.SaveChangesAsync();
            return Ok(200);
        }

        [HttpGet]
        [Route("GetComentariosPorUsuario/{usuarioId}")]
        public async Task<IActionResult> GetComentariosPorUsuario(int usuarioId)
        {
            var dataDB = await (from c in _context.Comentarios where c.UsuarioId == usuarioId select c).ToListAsync();
            return Ok(dataDB);
        }
    }
}
