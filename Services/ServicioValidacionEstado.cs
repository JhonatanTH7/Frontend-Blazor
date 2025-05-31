using Microsoft.JSInterop;
using System.Threading.Tasks;
using System; // Necesario para Action

namespace FrontBlazor.Services
{
    public class ServicioValidacionEstado
    {
        private readonly IJSRuntime _jsRuntime;

        // Evento que otros componentes pueden suscribirse para ser notificados de cambios
        public event Action? OnChange;

        private string? _correoUsuario; // Campo privado que almacena el correo del usuario
        
        // Propiedad pública para acceder y modificar el correo del usuario
        // El 'set' es privado para que solo el servicio pueda modificarlo directamente,
        // asegurando que NotifyStateChanged siempre sea llamado.
        public string? CorreoUsuario
        {
            get => _correoUsuario;
            private set // Setter privado, solo accesible desde dentro de esta clase
            {
                // Solo notificar si el valor realmente ha cambiado
                if (_correoUsuario != value)
                {
                    _correoUsuario = value;
                    NotifyStateChanged(); // ¡Esto es lo que notifica a los suscriptores!
                }
            }
        }

        public ServicioValidacionEstado(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        // Método para cargar el correo del usuario desde sessionStorage al inicio
        // Es importante llamar a este método en OnInitializedAsync de MainLayout y NavMenu.
        public async Task LoadCorreoUsuario()
        {
            // Asignar al setter de la propiedad para que se maneje la notificación
            // Esto es importante porque si _correoUsuario ya tiene el valor (ej. de una recarga),
            // el setter no notificará si el valor es el mismo.
            CorreoUsuario = await _jsRuntime.InvokeAsync<string>("sessionStorage.getItem", "usuarioEmail");
        }

        // Método para establecer el correo (ej. al iniciar sesión)
        // Renombrado a SetUserSession para mayor claridad y consistencia.
        public async Task SetUserSession(string email)
        {
            // Guarda el email en sessionStorage
            await _jsRuntime.InvokeVoidAsync("sessionStorage.setItem", "usuarioEmail", email);
            // Asigna el email a la propiedad CorreoUsuario.
            // El setter de CorreoUsuario se encargará de actualizar _correoUsuario y llamar a NotifyStateChanged().
            CorreoUsuario = email; 
        }

        // Método para limpiar la sesión (ej. al cerrar sesión)
        public async Task ClearSession()
        {
            // Limpia el email de sessionStorage
            await _jsRuntime.InvokeVoidAsync("sessionStorage.removeItem", "usuarioEmail"); // Mejor removeItem para solo el email
            // Opcional: limpiar también los roles y rutas si son relevantes para la sesión
            await _jsRuntime.InvokeVoidAsync("eval", @"
                Object.keys(sessionStorage)
                      .filter(k => k.startsWith('rol_') || k.startsWith('ruta_'))
                      .forEach(k => sessionStorage.removeItem(k));
            ");
            
            // Asigna null a la propiedad CorreoUsuario para reflejar que no hay sesión.
            // El setter de CorreoUsuario se encargará de actualizar _correoUsuario y llamar a NotifyStateChanged().
            CorreoUsuario = null; 
        }

        // Método privado para disparar el evento (solo llamado desde el setter de CorreoUsuario)
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}