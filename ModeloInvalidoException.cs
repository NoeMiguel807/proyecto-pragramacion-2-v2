using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlenderMarket.Excepciones
{
    public class ModeloInvalidoException : Exception
    {
        public string Campo { get; }
        public string Valor { get; }

        public ModeloInvalidoException(string campo, string valor, string mensaje)
            : base(mensaje)
        {
            Campo = campo;
            Valor = valor;
        }
    }
}
