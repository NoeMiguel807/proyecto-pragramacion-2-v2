using BlenderMarket.Modelos;
using System;
using System.Diagnostics;
using System.Text;
using System.Windows;

namespace BlenderMarket
{
    public partial class VentanaPago : Window
    {
        private CarritoCompras carrito;   // ← clase correcta

        public VentanaPago(CarritoCompras carrito)   // ← clase correcta
        {
            InitializeComponent();
            this.carrito = carrito;
        }

        private void BtnEnviarWhatsApp_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombre.Text.Trim();
            string correo = txtCorreo.Text.Trim();
            string telefono = txtTelefono.Text.Trim();

            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(telefono))
            {
                borderError.Visibility = Visibility.Visible;
                txtMensajeError.Text = "Por favor, completa todos los campos.";
                return;
            }

            // Construir mensaje
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("📦 *NUEVO PEDIDO DESDE BLENDER MARKET*");
            sb.AppendLine("");
            sb.AppendLine($"👤 *Nombre:* {nombre}");
            sb.AppendLine($"📧 *Correo:* {correo}");
            sb.AppendLine($"📱 *Teléfono:* {telefono}");
            sb.AppendLine("");
            sb.AppendLine("🛒 *Carrito:*");

            foreach (var item in carrito.Items)
            {
                sb.AppendLine($"• {item.Modelo.Nombre} (x{item.Cantidad}) — Bs. {item.Subtotal:F2}");
            }

            sb.AppendLine("");
            sb.AppendLine($"💰 *TOTAL:* Bs. {carrito.Total:F2}");

            string mensaje = Uri.EscapeDataString(sb.ToString());
            string telefonoDestino = "59160592868";

            string url = $"https://wa.me/{telefonoDestino}?text={mensaje}";

            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });

                this.DialogResult = true;
                this.Close();
            }
            catch
            {
                MessageBox.Show("Error al abrir WhatsApp", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
