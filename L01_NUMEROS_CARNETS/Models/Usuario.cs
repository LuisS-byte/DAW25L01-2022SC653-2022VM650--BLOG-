using System;
using System.Collections.Generic;

namespace L01_NUMEROS_CARNETS.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public int? RolId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;
}
