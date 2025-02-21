using L01_NUMEROS_CARNETS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace L01_NUMEROS_CARNETS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalificacionController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CalificacionController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("ListarCalificaciones")]
        public IActionResult ListarCalificaciones()
        {
            var listadoCalificaciones = (from c in _context.Calificaciones
                                         join p in _context.Publicaciones on c.PublicacionId equals p.PublicacionId
                                         join u in _context.Usuarios on c.UsuarioId equals u.UsuarioId
                                         select new
                                         {
                                             CalificacionId = c.CalificacionId,
                                             Publicacion = p.Titulo,
                                             Usuario = u.Nombre + " " + u.Apellido,
                                             Puntaje = c.Calificacion
                                         }).ToList();

            if (listadoCalificaciones.Count == 0)
            {
                return NotFound();
            }

            return Ok(listadoCalificaciones);
        }

        [HttpGet]
        [Route("ObtenerPorPublicacion/{publicacionId}")]
        public IActionResult ObtenerPorPublicacion(int publicacionId)
        {
            var calificaciones = (from c in _context.Calificaciones
                                  join p in _context.Publicaciones on c.PublicacionId equals p.PublicacionId
                                  join u in _context.Usuarios on c.UsuarioId equals u.UsuarioId
                                  where c.PublicacionId == publicacionId
                                  select new
                                  {
                                      CalificacionId = c.CalificacionId,
                                      Publicacion = p.Titulo,
                                      Usuario = u.Nombre + " " + u.Apellido,
                                      Puntaje = c.Calificacion
                                  }).ToList();

            if (calificaciones.Count == 0)
            {
                return NotFound();
            }

            return Ok(calificaciones);
        }

        [HttpPost]
        [Route("AgregarCalificacion")]
        public IActionResult AgregarCalificacion([FromBody] Calificacione nuevaCalificacion)
        {
            _context.Calificaciones.Add(nuevaCalificacion);
            _context.SaveChanges();
            return Ok(nuevaCalificacion);
        }

        [HttpPut]
        [Route("ActualizarCalificacion/{id}")]
        public IActionResult ActualizarCalificacion(int id, [FromBody] Calificacione calificacionActualizada)
        {
            var calificacion = _context.Calificaciones.FirstOrDefault(c => c.CalificacionId == id);
            if (calificacion == null)
            {
                return NotFound();
            }

            calificacion.PublicacionId = calificacionActualizada.PublicacionId;
            calificacion.UsuarioId = calificacionActualizada.UsuarioId;
            calificacion.Calificacion = calificacionActualizada.Calificacion;

            _context.SaveChanges();
            return Ok(calificacion);
        }

        [HttpDelete]
        [Route("EliminarCalificacion/{id}")]
        public IActionResult EliminarCalificacion(int id)
        {
            var calificacion = _context.Calificaciones.FirstOrDefault(c => c.CalificacionId == id);
            if (calificacion == null)
            {
                return NotFound();
            }

            _context.Calificaciones.Remove(calificacion);
            _context.SaveChanges();
            return Ok();
        }
    }
}
