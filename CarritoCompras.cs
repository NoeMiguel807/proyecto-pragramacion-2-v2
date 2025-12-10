using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlenderMarket.Modelos
{
    // AGREGACIÓN y COMPOSICIÓN
    public class CarritoCompras
    {
        // AGREGACIÓN: El carrito tiene items pero no es dueño de su ciclo de vida
        private List<ItemCarrito> _items;

        public IReadOnlyList<ItemCarrito> Items => _items.AsReadOnly();
        public decimal Total => _items.Sum(i => i.Subtotal);
        public int CantidadItems => _items.Count;

        public CarritoCompras()
        {
            _items = new List<ItemCarrito>();
        }

        public void AgregarItem(ModeloBlender modelo, int cantidad = 1)
        {
            var itemExistente = _items.FirstOrDefault(i => i.Modelo.Id == modelo.Id);

            if (itemExistente != null)
            {
                itemExistente.Cantidad += cantidad;
            }
            else
            {
                // COMPOSICIÓN: ItemCarrito existe solo dentro del carrito
                _items.Add(new ItemCarrito(modelo, cantidad));
            }
        }

        public void RemoverItem(string modeloId)
        {
            var item = _items.FirstOrDefault(i => i.Modelo.Id == modeloId);
            if (item != null)
                _items.Remove(item);
        }

        public void VaciarCarrito()
        {
            _items.Clear();
        }
    }

    // Clase interna - COMPOSICIÓN
    public class ItemCarrito
    {
        public ModeloBlender Modelo { get; }
        public int Cantidad { get; set; }
        public decimal Subtotal => Modelo.Precio * Cantidad;

        public ItemCarrito(ModeloBlender modelo, int cantidad)
        {
            Modelo = modelo;
            Cantidad = cantidad;
        }
    }
}
