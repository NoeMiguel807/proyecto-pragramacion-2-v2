using BlenderMarket.Modelos;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlenderMarket.Models
{
    public class UsuarioRegistrado : Usuario
    {
        public string Username { get; set; }

        // ¡IMPORTANTE! Propiedad pública sin enmascaramiento
        public string Password { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Sobrescribiendo propiedad abstracta
        public override string TipoUsuario => "Registrado";

        // Sobrescribiendo método virtual
        public override bool PuedeSubirModelos() => true;

        // SOBRECARGA de constructores
        public UsuarioRegistrado() { }

        public UsuarioRegistrado(string username, string password, string email, string nombre)
        {
            Username = username;
            Password = password;  // Guarda la contraseña real
            Email = email;
            Nombre = nombre;
        }
    }
}