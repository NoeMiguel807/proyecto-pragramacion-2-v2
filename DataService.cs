using BlenderMarket.Modelos;
using BlenderMarket.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BlenderMarket.Services
{
    public class DataService
    {
        private const string USUARIOS_FILE = "usuarios.json";
        private const string MODELOS_FILE = "modelos.json";

        // SINGLETON: Solo una instancia en toda la aplicación
        private static DataService _instance;
        public static DataService Instance => _instance ??= new DataService();

        private DataService()
        {
            EnsureFilesExist();
        }

        private void EnsureFilesExist()
        {
            if (!File.Exists(USUARIOS_FILE))
                File.WriteAllText(USUARIOS_FILE, "[]");

            if (!File.Exists(MODELOS_FILE))
                File.WriteAllText(MODELOS_FILE, "[]");
        }

        // ========== MÉTODOS PARA USUARIOS ==========

        public List<UsuarioRegistrado> CargarUsuarios()
        {
            try
            {
                var json = File.ReadAllText(USUARIOS_FILE);
                return JsonConvert.DeserializeObject<List<UsuarioRegistrado>>(json)
                    ?? new List<UsuarioRegistrado>();
            }
            catch (Exception)
            {
                return new List<UsuarioRegistrado>();
            }
        }

        public void GuardarUsuarios(List<UsuarioRegistrado> usuarios)
        {
            try
            {
                var json = JsonConvert.SerializeObject(usuarios, Formatting.Indented);
                File.WriteAllText(USUARIOS_FILE, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar usuarios: {ex.Message}");
            }
        }

        public void GuardarUsuario(UsuarioRegistrado usuario)
        {
            try
            {
                var usuarios = CargarUsuarios();
                var existente = usuarios.FirstOrDefault(u => u.Id == usuario.Id);

                if (existente != null)
                {
                    // Reemplazar usuario existente
                    usuarios.Remove(existente);
                }

                usuarios.Add(usuario);
                GuardarUsuarios(usuarios);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar usuario: {ex.Message}");
            }
        }

        public UsuarioRegistrado BuscarUsuario(string username, string password)
        {
            try
            {
                var usuarios = CargarUsuarios();

                // Depuración: Mostrar usuarios cargados
                Console.WriteLine($"=== BUSCANDO USUARIO ===");
                Console.WriteLine($"Buscando: Usuario='{username}', Password='{password}'");
                Console.WriteLine($"Total usuarios en DB: {usuarios.Count}");

                foreach (var user in usuarios)
                {
                    Console.WriteLine($"Usuario en DB: '{user.Username}', Password: '{user.Password}'");
                }

                // Buscar usuario que coincida EXACTAMENTE
                var usuarioEncontrado = usuarios.FirstOrDefault(u =>
                    u.Username == username &&
                    u.Password == password);

                Console.WriteLine($"Usuario encontrado: {usuarioEncontrado != null}");

                return usuarioEncontrado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar usuario: {ex.Message}");
                return null;
            }
        }

        public bool UsuarioExiste(string username)
        {
            try
            {
                var usuarios = CargarUsuarios();
                return usuarios.Any(u => u.Username == username);
            }
            catch (Exception)
            {
                return false;
            }
        }

        // ========== MÉTODOS PARA MODELOS ==========

        public List<ModeloBlender> CargarModelos()
        {
            try
            {
                var json = File.ReadAllText(MODELOS_FILE);
                return JsonConvert.DeserializeObject<List<ModeloBlender>>(json)
                    ?? new List<ModeloBlender>();
            }
            catch (Exception)
            {
                return new List<ModeloBlender>();
            }
        }

        public void GuardarModelos(List<ModeloBlender> modelos)
        {
            try
            {
                var json = JsonConvert.SerializeObject(modelos, Formatting.Indented);
                File.WriteAllText(MODELOS_FILE, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar modelos: {ex.Message}");
            }
        }

        public void AgregarModelo(ModeloBlender modelo)
        {
            try
            {
                var modelos = CargarModelos();
                modelos.Add(modelo);
                GuardarModelos(modelos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar modelo: {ex.Message}");
            }
        }

        public List<ModeloBlender> BuscarModelos(string categoria = null)
        {
            try
            {
                var modelos = CargarModelos();

                if (string.IsNullOrEmpty(categoria))
                    return modelos;

                return modelos.Where(m => m.Categoria == categoria).ToList();
            }
            catch (Exception)
            {
                return new List<ModeloBlender>();
            }
        }
    }
}