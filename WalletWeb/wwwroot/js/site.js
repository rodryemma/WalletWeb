// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

async function apiRequest(url, method = "GET", data = null) {
    try {
        const options = {
            method,
            headers: { "Content-Type": "application/json" }
        };

        if (data) {
            options.body = JSON.stringify(data);
        }

        const response = await fetch(url, options);

        const contentType = response.headers.get("content-type");
        const isJson = contentType && contentType.includes("application/json");

        if (!response.ok) {
            const errorData = isJson ? await response.json() : await response.text();
            let mensajeError = 'Error en la respuesta';

            if (errorData?.errors && Array.isArray(errorData.errors)) {
                mensajeError = errorData.errors.join('\n');
            } else if (typeof errorData === 'string') {
                mensajeError = errorData;
            }
            throw new Error(mensajeError);
        }

        return isJson ? await response.json() : null;

    } catch (error) {
        console.error("Error en apiRequest:", error);
        throw error; 
    }
}

function mostrarToast(mensaje, tipo = 'success') {
    const toastEl = document.getElementById('toastMensaje');
    const toastTexto = document.getElementById('toastMensajeTexto');

    toastTexto.textContent = mensaje;

    // Reemplazar clases visuales
    //toastEl.className = `toast align-items-center text-bg-${tipo} bg-primary border-0`;

    //toastEl.className = 'toast align-items-center text-white border-0 shadow rounded';
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

