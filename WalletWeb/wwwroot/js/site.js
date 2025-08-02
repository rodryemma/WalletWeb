// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function mostrarToast(mensaje, tipo = 'success') {
    const toastEl = document.getElementById('toastMensaje');
    const toastTexto = document.getElementById('toastMensajeTexto');

    toastTexto.textContent = mensaje;

    // Reemplazar clases visuales
    //toastEl.className = `toast align-items-center text-bg-${tipo} bg-primary border-0`;

    toastEl.className = 'toast align-items-center text-white border-0 shadow rounded';

    // Agregar color de fondo según tipo (success, danger, warning, info)
    switch (tipo) {
        case 'success':
            toastEl.classList.add('bg-success');
            break;
        case 'danger':
            toastEl.classList.add('bg-danger');
            break;
        case 'warning':
            toastEl.classList.add('bg-warning', 'text-dark');
            break;
        case 'info':
            toastEl.classList.add('bg-info');
            break;
        default:
            toastEl.classList.add('bg-secondary');
    }

    // Inicializar con autohide y duración
    const toast = new bootstrap.Toast(toastEl, {
        autohide: true,
        delay: 2000
    });

    toast.show();
}

