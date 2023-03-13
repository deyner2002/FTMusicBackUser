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

        [Required(ErrorMessage = "El Nombre del usuario es requerido")]
        public string? Nombre { get; set; }
        [Required(ErrorMessage = "El Correo del usuario es requerido")]
        //[RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "El correo no es valido")]
        public string? Correo { get; set; }
        [Required(ErrorMessage = "La Contrasenia del usuario es requerida")]
        public string? Contrasenia { get; set; }
        public DateTime? FechaRegistro { get; set; }
        [Required(ErrorMessage = "La descripcion del usuario es requerida")]
        public string? Descripcion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public string? Facebook { get; set; }
        public string? Instagram { get; set; }
        public string? Youtube { get; set; }
    }


}
