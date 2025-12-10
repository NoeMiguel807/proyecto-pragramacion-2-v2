using BlenderMarket.Modelos;
using BlenderMarket.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BlenderMarket.Interfaces;

namespace BlenderMarket
{
    public partial class PaginaCatalogo : UserControl
    {
        private List<ModeloBlender> _todosModelos = new List<ModeloBlender>();
        private bool _cargado = false;

        public PaginaCatalogo()
        {
            InitializeComponent();
            // Esperar a que se cargue para inicializar controles
            this.Loaded += PaginaCatalogo_Loaded;
        }

        private void PaginaCatalogo_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_cargado)
            {
                CargarModelos();
                _cargado = true;
            }
        }

        private void CargarModelos()
        {
            try
            {
                var dataService = DataService.Instance;
                _todosModelos = dataService.CargarModelos();
                ActualizarLista();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar modelos: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ActualizarLista()
        {
            try
            {
                // Verificar que los controles estén listos
                if (cmbOrdenar == null || !cmbOrdenar.IsLoaded)
                    return;

                var modelosFiltrados = FiltrarModelos();
                var modelosOrdenados = OrdenarModelos(modelosFiltrados);

                itemsControlModelos.ItemsSource = modelosOrdenados;

                if (modelosOrdenados.Any())
                {
                    borderSinModelos.Visibility = Visibility.Collapsed;
                    itemsControlModelos.Visibility = Visibility.Visible;
                }
                else
                {
                    borderSinModelos.Visibility = Visibility.Visible;
                    itemsControlModelos.Visibility = Visibility.Collapsed;
                }

                txtContador.Text = $"{modelosOrdenados.Count} modelo(s)";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al actualizar lista: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<ModeloBlender> FiltrarModelos()
        {
            var filtrados = _todosModelos;

            if (!string.IsNullOrWhiteSpace(txtBusqueda.Text))
            {
                var busqueda = txtBusqueda.Text.ToLower();
                filtrados = filtrados.Where(m =>
                    m.Nombre.ToLower().Contains(busqueda) ||
                    m.Descripcion.ToLower().Contains(busqueda) ||
                    m.Categoria.ToLower().Contains(busqueda) ||
                    m.Tipo.ToLower().Contains(busqueda)
                ).ToList();
            }

            if (cmbFiltroCategoria != null && cmbFiltroCategoria.SelectedItem is ComboBoxItem categoriaItem &&
                categoriaItem.Content.ToString() != "Todas las categorías")
            {
                var categoria = categoriaItem.Content.ToString();
                filtrados = filtrados.Where(m => m.Categoria == categoria).ToList();
            }

            return filtrados;
        }

        private List<ModeloBlender> OrdenarModelos(List<ModeloBlender> modelos)
        {
            // Verificación segura del ComboBox
            if (cmbOrdenar == null || cmbOrdenar.SelectedItem == null)
                return modelos.OrderByDescending(m => m.FechaSubida).ToList();

            if (cmbOrdenar.SelectedItem is ComboBoxItem ordenItem)
            {
                switch (ordenItem.Content.ToString())
                {
                    case "Más descargados":
                        return modelos.OrderByDescending(m => m.ContadorDescargas).ToList();
                    case "Precio: Menor a Mayor":
                        return modelos.OrderBy(m => m.Precio).ToList();
                    case "Precio: Mayor a Menor":
                        return modelos.OrderByDescending(m => m.Precio).ToList();
                    case "A-Z":
                        return modelos.OrderBy(m => m.Nombre).ToList();
                    case "Más recientes":
                    default:
                        return modelos.OrderByDescending(m => m.FechaSubida).ToList();
                }
            }

            return modelos.OrderByDescending(m => m.FechaSubida).ToList();
        }

        // ===== EVENTOS =====
        private void TxtBusqueda_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_cargado) ActualizarLista();
        }

        private void CmbFiltroCategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cargado) ActualizarLista();
        }

        private void CmbOrdenar_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cargado) ActualizarLista();
        }

        private void BtnAgregarCarrito_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag is string modeloId)
                {
                    var modelo = _todosModelos.FirstOrDefault(m => m.Id == modeloId);

                    if (modelo != null)
                    {
                        var mainWindow = MainWindow.Instance;

                        if (mainWindow.UsuarioActual?.PuedeComprar() == true)
                        {
                            mainWindow.Carrito.AgregarItem(modelo);
                            mainWindow.ActualizarUI();

                            MessageBox.Show($"✅ '{modelo.Nombre}' agregado al carrito",
                                "Carrito", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("❌ Debes iniciar sesión para comprar modelos",
                                "Carrito", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDescargar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender is Button button && button.Tag is string modeloId)
                {
                    var modelo = _todosModelos.FirstOrDefault(m => m.Id == modeloId);

                    if (modelo != null)
                    {
                        modelo.IncrementarDescarga();

                        var dataService = DataService.Instance;
                        var todosModelos = dataService.CargarModelos();
                        dataService.GuardarModelos(todosModelos);

                        MessageBox.Show(
                            $"📥 Descarga iniciada para: {modelo.Nombre}\n\n" +
                            $"Total descargas: {modelo.ContadorDescargas}",
                            "Descarga",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);

                        ActualizarLista();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al descargar: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}