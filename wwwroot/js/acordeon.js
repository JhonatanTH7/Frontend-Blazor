// wwwroot/js/accordion.js

// Importante: Asigna la función al objeto 'window' para que Blazor pueda llamarla.
window.initializeAccordions = function() {
    console.log("initializeAccordions called!"); // Mensaje de depuración
    var acc = document.getElementsByClassName("accordion");
    console.log("Number of accordions found: " + acc.length); // Mensaje de depuración

    for (var i = 0; i < acc.length; i++) {
        // Asigna el manejador de clic directamente al evento 'onclick'
        // Esto sobrescribe cualquier manejador anterior y es seguro para re-renderizados
        acc[i].onclick = function() {
            console.log("Accordion clicked!"); // Mensaje de depuración

            // 1. Alterna la clase 'active' en el botón del acordeón
            this.classList.toggle("active");

            // 2. Obtiene el panel de contenido (el elemento siguiente al botón)
            var panel = this.nextElementSibling;

            // 3. Verifica si el panel existe y tiene la clase 'panel'
            if (panel && panel.classList.contains("panel")) {
                if (panel.style.maxHeight) {
                    // Si el panel tiene un maxHeight (está abierto), lo cierra.
                    // Establecer a null hace que la CSS transition a max-height: 0 se active.
                    panel.style.maxHeight = null; 
                } else {
                    // Si el panel está cerrado, lo abre.
                    // Establece el maxHeight al scrollHeight para que todo el contenido sea visible.
                    panel.style.maxHeight = panel.scrollHeight + "px";
                }
            } else {
                console.warn("Error: El elemento siguiente al acordeón no es un panel o no existe.", this);
            }
        };
    }
};