using BlenderMarket.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BlenderMarket
{
    public partial class PaginaInicio : UserControl
    {
        public PaginaInicio()
        {
            InitializeComponent();
        }

        private void BtnIniciarSesion_Click(object sender, RoutedEventArgs e)
        {
            var ventanaLogin = new VentanaLogin();
            ventanaLogin.Owner = Window.GetWindow(this);

            if (ventanaLogin.ShowDialog() == true)
            {
                // El login fue exitoso
                MainWindow.Instance.NavigateToCatalogo();
            }
        }

        private void BtnRegistrarse_Click(object sender, RoutedEventArgs e)
        {
            var ventanaRegistro = new VentanaRegistro();
            ventanaRegistro.Owner = Window.GetWindow(this);
            ventanaRegistro.ShowDialog();
        }

        private void BtnInvitado_Click(object sender, RoutedEventArgs e)
        {
            // Crear usuario invitado
            MainWindow.Instance.UsuarioActual = new UsuarioInvitado
            {
                Nombre = "Invitado",
                Email = "invitado@blendermarket.com"
            };

            // Ir al catálogo
            MainWindow.Instance.NavigateToCatalogo();
        }
    }
}