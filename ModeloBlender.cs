using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlenderMarket.Interfaces;

namespace BlenderMarket.Modelos
{
        public class ModeloBlender : IContadorDescargas
        {
            public string Id { get; set; } = Guid.NewGuid().ToString();
            public string Nombre { get; set; }
            public string Tipo { get; set; }
            public string Categoria { get; set; }
            public string Descripcion { get; set; }
            public decimal Precio { get; set; }
            public string RutaArchivo { get; set; }
            public string NombreArchivo { get; set; }
            public DateTime FechaSubida { get; set; } = DateTime.Now;
            public string IdUsuarioSubio { get; set; }

            // Implementación de INTERFACE
            private int _contadorDescargas;
            public int ContadorDescargas
            {
                get => _contadorDescargas;
                private set => _contadorDescargas = value;
            }

            public void IncrementarDescarga() => ContadorDescargas++;
            public void ReiniciarContador() => ContadorDescargas = 0;

            // Método para validar
            public void ValidarModelo()
            {
                if (string.IsNullOrWhiteSpace(Nombre))
                    throw new ArgumentException("El nombre del modelo es requerido");

                if (Precio < 0)
                    throw new ArgumentException("El precio no puede ser negativo");

                if (string.IsNullOrWhiteSpace(Tipo))
                    throw new ArgumentException("El tipo del modelo es requerido");
            }
        }
     
}
