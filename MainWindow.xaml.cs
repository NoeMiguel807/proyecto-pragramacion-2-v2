using BlenderMarket.Modelos;
using BlenderMarket.Models;
using System.Windows;
using System.Windows.Controls;

namespace BlenderMarket
{
    public partial class MainWindow : Window
    {
        private static MainWindow _instance;
        public static MainWindow Instance => _instance;

        public Usuario UsuarioActual { get; set; }
        public CarritoCompras Carrito { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            _instance = this;
            Carrito = new CarritoCompras();
        }

        // 🔥 ESTE MÉTODO ES EL QUE HACE QUE PAGINAINICIO SE CARGUE DE VERDAD
        private void MainFrame_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateToInicio();
        }

        public void NavigateToInicio()
        {
            var paginaInicio = new PaginaInicio();
            MainFrame.Navigate(paginaInicio);
            ActualizarUI();
        }

        public void NavigateToCatalogo()
        {
            var paginaCatalogo = new PaginaCatalogo();
            MainFrame.Navigate(paginaCatalogo);
        }

        public void NavigateToCarrito()
        {
            var paginaCarrito = new PaginaCarrito();
            MainFrame.Navigate(paginaCarrito);
        }

        public void ActualizarUI()
        {
            if (UsuarioActual != null)
            {
                txtEstadoSesion.Text = $"👤 {UsuarioActual.Nombre} ({UsuarioActual.TipoUsuario})";
                btnCerrarSesion.Visibility = Visibility.Visible;

                if (UsuarioActual is UsuarioRegistrado)
                    btnSubirModelo.Visibility = Visibility.Visible;
                else
                    btnSubirModelo.Visibility = Visibility.Collapsed;
            }
            else
            {
                txtEstadoSesion.Text = "No conectado";
                btnCerrarSesion.Visibility = Visibility.Collapsed;
                btnSubirModelo.Visibility = Visibility.Collapsed;
            }

            btnCarrito.Content = $"🛒 Carrito ({Carrito.CantidadItems})";
        }

        private void BtnInicio_Click(object sender, RoutedEventArgs e)
        {
            NavigateToInicio();
        }

        private void BtnCatalogo_Click(object sender, RoutedEventArgs e)
        {
            NavigateToCatalogo();
        }

        private void BtnCarrito_Click(object sender, RoutedEventArgs e)
        {
            NavigateToCarrito();
        }

        private void BtnSubirModelo_Click(object sender, RoutedEventArgs e)
        {
            if (UsuarioActual is UsuarioRegistrado)
            {
                var ventanaSubir = new VentanaSubirModelo();
                ventanaSubir.Owner = this;
                ventanaSubir.ShowDialog();

                if (ventanaSubir.ModeloSubido)
                    NavigateToCatalogo();
            }
            else
            {
                MessageBox.Show("❌ Debes iniciar sesión para subir modelos",
                    "Acceso Denegado", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnCerrarSesion_Click(object sender, RoutedEventArgs e)
        {
            UsuarioActual = null;
            Carrito.VaciarCarrito();
            NavigateToInicio();
        }
    }
}
