

function deshabilitarPorRol(idBoton) {
  const rol_Validador = sessionStorage.getItem('rol_Validador');
  const button = document.getElementById('idBoton');
    if (rol_Validador === 'Validador') {
        button.disabled = true;
    }
}