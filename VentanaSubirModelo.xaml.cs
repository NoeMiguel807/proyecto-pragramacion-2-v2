using BlenderMarket.Modelos;
using BlenderMarket.Models;
using BlenderMarket.Services;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace BlenderMarket
{
    public partial class VentanaSubirModelo : Window
    {
        private string _rutaArchivoSeleccionado = "";
        public bool ModeloSubido { get; private set; } = false;

        public VentanaSubirModelo()
        {
            InitializeComponent();

            // Seleccionar primeros items por defecto
            if (cmbTipo != null)
                cmbTipo.SelectedIndex = 0;

            if (cmbCategoria != null)
                cmbCategoria.SelectedIndex = 0;
        }

        private void BtnSeleccionarArchivo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Archivos 3D|*.blend;*.fbx;*.obj;*.stl|Todos los archivos|*.*",
                    Title = "Seleccionar archivo del modelo 3D"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    _rutaArchivoSeleccionado = openFileDialog.FileName;
                    txtNombreArchivo.Text = Path.GetFileName(_rutaArchivoSeleccionado);

                    // Si no hay nombre, sugerir uno basado en el archivo
                    if (string.IsNullOrWhiteSpace(txtNombre.Text))
                    {
                        txtNombre.Text = Path.GetFileNameWithoutExtension(_rutaArchivoSeleccionado);
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarError($"Error al seleccionar archivo: {ex.Message}");
            }
        }

        private void BtnSubir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 1. VALIDAR USUARIO AUTORIZADO
                var mainWindow = MainWindow.Instance;
                if (mainWindow == null)
                    throw new Exception("❌ No se pudo acceder a la ventana principal");

                var usuarioActual = mainWindow.UsuarioActual as UsuarioRegistrado;
                if (usuarioActual == null)
                    throw new Exception("❌ Debes estar registrado para subir modelos");

                // 2. VALIDAR CAMPOS REQUERIDOS
                if (string.IsNullOrWhiteSpace(txtNombre.Text))
                    throw new Exception("❌ El nombre del modelo es requerido");

                if (cmbTipo == null || cmbTipo.SelectedItem == null)
                    throw new Exception("❌ Debes seleccionar un tipo");

                if (string.IsNullOrWhiteSpace(_rutaArchivoSeleccionado))
                    throw new Exception("❌ Debes seleccionar un archivo");

                // 3. VALIDAR PRECIO
                if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio < 0)
                    throw new Exception("❌ El precio debe ser un número válido y no negativo");

                // 4. VALIDAR NOMBRE (no números solos)
                if (Regex.IsMatch(txtNombre.Text, @"^\d+$"))
                    throw new Exception("❌ El nombre no puede ser solo números");

                // 5. OBTENER VALORES DE LOS COMBOBOX
                string tipoSeleccionado = "";
                string categoriaSeleccionada = "";

                if (cmbTipo.SelectedItem is ComboBoxItem tipoItem)
                    tipoSeleccionado = tipoItem.Content?.ToString() ?? "";

                if (cmbCategoria.SelectedItem is ComboBoxItem categoriaItem)
                    categoriaSeleccionada = categoriaItem.Content?.ToString() ?? "";

                // 6. CREAR NUEVO MODELO
                var nuevoModelo = new ModeloBlender
                {
                    Nombre = txtNombre.Text.Trim(),
                    Tipo = tipoSeleccionado,
                    Categoria = categoriaSeleccionada,
                    Descripcion = txtDescripcion.Text.Trim(),
                    Precio = precio,
                    RutaArchivo = _rutaArchivoSeleccionado,
                    NombreArchivo = Path.GetFileName(_rutaArchivoSeleccionado),
                    IdUsuarioSubio = usuarioActual.Id
                };

                // 7. VALIDAR MODELO
                nuevoModelo.ValidarModelo();

                // 8. GUARDAR MODELO
                var dataService = DataService.Instance;
                dataService.AgregarModelo(nuevoModelo);

                // 9. MOSTRAR MENSAJE DE ÉXITO
                MessageBox.Show(
                    $"✅ Modelo '{nuevoModelo.Nombre}' subido exitosamente!\n\n" +
                    $"Tipo: {nuevoModelo.Tipo}\n" +
                    $"Precio: ${nuevoModelo.Precio:F2}\n" +
                    $"¡Ahora está disponible en el catálogo!",
                    "Modelo Subido",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // 10. MARCAR COMO SUBIDO Y CERRAR
                ModeloSubido = true;
                DialogResult = true;
                Close();
            }
            catch (FormatException)
            {
                MostrarError("❌ El precio debe ser un número válido (ej: 19.99)");
            }
            catch (ArgumentException ex)
            {
                MostrarError($"❌ Error de validación: {ex.Message}");
            }
            catch (Exception ex)
            {
                MostrarError($"❌ Error: {ex.Message}");
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

        private void LimpiarError(object sender, RoutedEventArgs e)
        {
            borderError.Visibility = Visibility.Collapsed;
        }
    }
}