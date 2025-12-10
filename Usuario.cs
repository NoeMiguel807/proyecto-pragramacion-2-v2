using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlenderMarket.Modelos
{
    // HERENCIA: Esta es la clase PADRE (abstracta)
    public abstract class Usuario
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Nombre { get; set; }
        public string Email { get; set; }

        // Propiedad abstracta - las hijas DEBEN implementarla
        public abstract string TipoUsuario { get; }

        // Método virtual - las hijas PUEDEN cambiarlo
        public virtual bool PuedeSubirModelos() => false;

        // Método virtual - las hijas PUEDEN cambiarlo
        public virtual bool PuedeComprar() => true;

        // SOBRECARGA: Dos métodos con mismo nombre pero diferentes parámetros
        public string ObtenerInfo()
        {
            return $"{Nombre} ({Email})";
        }

        public string ObtenerInfo(bool detallado)
        {
            if (detallado)
                return $"{Nombre} - {Email} - {TipoUsuario}";
            else
                return ObtenerInfo();
        }
    }
}
