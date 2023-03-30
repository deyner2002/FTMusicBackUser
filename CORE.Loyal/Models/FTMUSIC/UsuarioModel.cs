using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Loyal.Models.FTMUSIC
{
    public class UsuarioModel
    {
        public long? Id { get; set; }
        public string? Nombre { get; set; }
        public string? NombreArtistico { get; set;}
        public string? Correo { get; set; }
        public string? Contrasenia { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string? Descripcion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Youtube { get; set; }
        public string? Estado { get; set; }
    }


}
