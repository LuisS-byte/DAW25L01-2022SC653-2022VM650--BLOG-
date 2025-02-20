using System;
using System.Collections.Generic;

namespace L01_NUMEROS_CARNETS.Models;

public partial class Comentario
{
    public int CometarioId { get; set; }

    public int? PublicacionId { get; set; }

    public string? Comentario1 { get; set; }

    public int? UsuarioId { get; set; }
}
