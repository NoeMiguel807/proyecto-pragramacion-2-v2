using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlenderMarket.Excepciones
{
    public class UsuarioNoAutorizadoException : Exception
    {
        public string Operacion { get; }

        public UsuarioNoAutorizadoException(string operacion)
            : base($"No autorizado para {operacion}")
        {
            Operacion = operacion;
        }
    }
}
