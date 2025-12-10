using BlenderMarket.Services;
using System;
using System.Text.RegularExpressions;
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
using System.Windows.Shapes;

namespace BlenderMarket
{
    public partial class VentanaLogin : Window
    {
        public VentanaLogin()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. VALIDACIONES BÁSICAS
                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                    throw new Exception("❌ El usuario es requerido");

                if (txtPassword.Password.Length == 0)
                    throw new Exception("❌ La contraseña es requerida");

                // 2. VALIDACIÓN CON EXPRESIÓN REGULAR
                if (!Regex.IsMatch(txtUsername.Text, @"^[a-zA-Z0-9_]{3,20}$"))
                    throw new Exception("❌ El usuario debe tener 3-20 caracteres\n(solo letras, números y _)");

                // 3. BUSCAR USUARIO
                var dataService = DataService.Instance;
                var usuario = dataService.BuscarUsuario(txtUsername.Text, txtPassword.Password);

                if (usuario != null)
                {
                    // 4. LOGIN EXITOSO
                    MainWindow.Instance.UsuarioActual = usuario;
                    DialogResult = true;
                    Close();
                }
                else
                {
                    throw new Exception("❌ Usuario o contraseña incorrectos");
                }
            }
            catch (Exception ex)
            {
                // 5. MANEJO DE EXCEPCIONES
                MostrarError(ex.Message);
            }
        }

        private void MostrarError(string mensaje)
        {
            txtMensajeError.Text = mensaje;
            borderError.Visibility = Visibility.Visible;
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        // Limpiar error cuando el usuario empieza a escribir
        private void TxtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            borderError.Visibility = Visibility.Collapsed;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            borderError.Visibility = Visibility.Collapsed;
        }
    }
}
