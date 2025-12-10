using BlenderMarket.Modelos;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BlenderMarket
{
    public partial class PaginaCarrito : UserControl
    {
        public PaginaCarrito()
        {
            InitializeComponent();
            ActualizarCarrito();
        }

        private void ActualizarCarrito()
        {
            var mainWindow = MainWindow.Instance;
            var carrito = mainWindow.Carrito;

            itemsControlCarrito.ItemsSource = carrito.Items;

            txtResumen.Text = $"{carrito.CantidadItems} item(s) - Bs. {carrito.Total:F2}";
            txtTotal.Text = $"Bs. {carrito.Total:F2}";

            if (carrito.CantidadItems > 0)
            {
                borderCarritoVacio.Visibility = Visibility.Collapsed;
                itemsControlCarrito.Visibility = Visibility.Visible;
                btnVaciarCarrito.IsEnabled = true;
                btnProcederPago.IsEnabled = mainWindow.UsuarioActual?.PuedeComprar() == true;
            }
            else
            {
                borderCarritoVacio.Visibility = Visibility.Visible;
                itemsControlCarrito.Visibility = Visibility.Collapsed;
                btnVaciarCarrito.IsEnabled = false;
                btnProcederPago.IsEnabled = false;
            }
        }

        private void BtnAumentarCantidad_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string modeloId)
            {
                var carrito = MainWindow.Instance.Carrito;
                var item = carrito.Items.FirstOrDefault(i => i.Modelo.Id == modeloId);

                if (item != null)
                {
                    carrito.AgregarItem(item.Modelo, 1);
                    ActualizarCarrito();
                    MainWindow.Instance.ActualizarUI();
                }
            }
        }

        private void BtnDisminuirCantidad_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string modeloId)
            {
                var carrito = MainWindow.Instance.Carrito;
                var item = carrito.Items.FirstOrDefault(i => i.Modelo.Id == modeloId);

                if (item != null && item.Cantidad > 1)
                {
                    carrito.RemoverItem(modeloId);
                    carrito.AgregarItem(item.Modelo, item.Cantidad - 1);
                }
                else if (item != null)
                {
                    carrito.RemoverItem(modeloId);
                }

                ActualizarCarrito();
                MainWindow.Instance.ActualizarUI();
            }
        }

        private void BtnEliminarItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string modeloId)
            {
                var resultado = MessageBox.Show(
                    "¿Estás seguro de eliminar este item del carrito?",
                    "Confirmar",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    MainWindow.Instance.Carrito.RemoverItem(modeloId);
                    ActualizarCarrito();
                    MainWindow.Instance.ActualizarUI();
                }
            }
        }

        private void BtnVaciarCarrito_Click(object sender, RoutedEventArgs e)
        {
            var resultado = MessageBox.Show(
                "¿Estás seguro de vaciar todo el carrito?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (resultado == MessageBoxResult.Yes)
            {
                MainWindow.Instance.Carrito.VaciarCarrito();
                ActualizarCarrito();
                MainWindow.Instance.ActualizarUI();
            }
        }

        private void BtnContinuarComprando_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.NavigateToCatalogo();
        }

        private void BtnIrCatalogo_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.Instance.NavigateToCatalogo();
        }

        private void BtnProcederPago_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = MainWindow.Instance;

            if (mainWindow.UsuarioActual == null)
            {
                MessageBox.Show("❌ Debes iniciar sesión para realizar una compra",
                    "Pago", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!mainWindow.UsuarioActual.PuedeComprar())
            {
                MessageBox.Show("❌ Los usuarios invitados no pueden realizar compras",
                    "Pago", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (mainWindow.Carrito.CantidadItems == 0)
            {
                MessageBox.Show("❌ Tu carrito está vacío",
                    "Pago", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var ventanaPago = new VentanaPago(mainWindow.Carrito);
            ventanaPago.Owner = Window.GetWindow(this);
            ventanaPago.ShowDialog();
        }
    }
}
