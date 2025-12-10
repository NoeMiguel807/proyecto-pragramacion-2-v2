using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlenderMarket.Modelos
{
    public class UsuarioInvitado : Usuario
    {
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        // Sobrescribiendo propiedad abstracta
        public override string TipoUsuario => "Invitado";

        // Sobrescribiendo método virtual
        public override bool PuedeComprar() => false;
    }
}
