using BlenderMarket.Models;
using BlenderMarket.Services;
using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace BlenderMarket
{
    public partial class VentanaRegistro : Window
    {
        public VentanaRegistro()
        {
            InitializeComponent();
        }

        private void BtnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. VALIDAR CAMPOS REQUERIDOS
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                    throw new Exception("❌ El nombre completo es requerido");

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                    throw new Exception("❌ El email es requerido");

                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                    throw new Exception("❌ El usuario es requerido");

                if (txtPassword.Password.Length == 0)
                    throw new Exception("❌ La contraseña es requerida");

                if (txtConfirmPassword.Password.Length == 0)
                    throw new Exception("❌ Debes confirmar la contraseña");

                // 2. VALIDAR CONTRASEÑAS COINCIDEN
                if (txtPassword.Password != txtConfirmPassword.Password)
                    throw new Exception("❌ Las contraseñas no coinciden");

                // 3. VALIDAR FORMATO DE EMAIL
                if (!Regex.IsMatch(txtEmail.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                    throw new Exception("❌ Email no válido");

                // 4. VALIDAR FORMATO DE USUARIO
                if (!Regex.IsMatch(txtUsername.Text, @"^[a-zA-Z0-9_]{3,20}$"))
                    throw new Exception("❌ El usuario debe tener 3-20 caracteres\n(solo letras, números y _)");

                // 5. VALIDAR CONTRASEÑA SEGURA
                if (txtPassword.Password.Length < 6)
                    throw new Exception("❌ La contraseña debe tener al menos 6 caracteres");

                // 6. VERIFICAR SI USUARIO YA EXISTE
                var dataService = DataService.Instance;
                if (dataService.UsuarioExiste(txtUsername.Text))
                    throw new Exception("❌ El usuario ya está registrado");

                // 7. CREAR NUEVO USUARIO - ¡CORREGIDO!
                var nuevoUsuario = new UsuarioRegistrado
                {
                    Nombre = txtNombre.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Password  // ← ¡GUARDA LA CONTRASEÑA REAL!
                };

                // 8. GUARDAR USUARIO
                dataService.GuardarUsuario(nuevoUsuario);

                // 9. MOSTRAR MENSAJE DE ÉXITO
                MessageBox.Show(
                    "✅ ¡Registro exitoso!\nAhora puedes iniciar sesión.",
                    "Registro Completado",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // 10. CERRAR VENTANA
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
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

        // Limpiar errores al escribir
        private void LimpiarError(object sender, RoutedEventArgs e)
        {
            borderError.Visibility = Visibility.Collapsed;
        }

        // Eventos para limpiar errores cuando se escribe
        private void TxtNombre_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            borderError.Visibility = Visibility.Collapsed;
        }

        private void TxtEmail_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            borderError.Visibility = Visibility.Collapsed;
        }

        private void TxtUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            borderError.Visibility = Visibility.Collapsed;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            borderError.Visibility = Visibility.Collapsed;
        }

        private void TxtConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            borderError.Visibility = Visibility.Collapsed;
        }
    }
}