using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlenderMarket.Interfaces
{
    interface IContadorDescargas
    {
        int ContadorDescargas { get; }
        void IncrementarDescarga();
        void ReiniciarContador();
    }
}
