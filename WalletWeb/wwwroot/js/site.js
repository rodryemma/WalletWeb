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

function buscarSeleccionarSelect2PorTexto(url, idselect, texto) {
    //Buscamos directamente por ajax la informacion y al obtenerla la cargamos nosotros y seleccionamos.
    $.ajax({
        url: url,
        dataType: 'json',
        data: { term: texto },
        success: function (data) {
            if (data.data && data.data.length > 0) {
                // Buscar item exacto
                const item = data.data.find(function (divisa) {
                    return divisa.nombre.trim().toLowerCase() === texto.trim().toLowerCase();
                });

                if (item) {
                    const $select = $(idselect);
                    // Crear la opción y la seleccionamos
                    const nuevaOpcion = new Option(item.nombre, item.id, true, true);
                    $select.append(nuevaOpcion);
                    $select.trigger('change');
                }
            }
        }
    });
}

// Región: Cargar Select Divisa
function cargarSelect2Divisa() {
    const url = '/divisa/json';
    var modalActivo = $('.modal.show').attr('id');
    $('#divisaSelectCrear , #divisaSelectEditar').select2({
        placeholder: 'Seleccionar...',
        dropdownParent: modalActivo ? $('#' + modalActivo) : $('body'),
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    term: params.term || ''
                };
            },
            processResults: function (data) {
                return {
                    results: data.data.map(function (item) {
                        return {
                            id: item.id,
                            text: item.nombre
                        };
                    })
                };
            },
            cache: true
        },
        minimumInputLength: 0
    });

    if (modalActivo === 'modalEditar') {
        const idSelect = '#divisaSelectEditar';
        const text = document.getElementById('editarSelectDivisa').value;
        buscarSeleccionarSelect2PorTexto(url, idSelect, text);
        document.getElementById('editarSelectDivisa').value = '';
    }
}

// Región: Cargar Select Cuenta
function cargarSelect2Cuenta() {
    const url = '/cuentawallet/json';
    var modalActivo = $('.modal.show').attr('id');
    $('#cuentaSelectCrear , #cuentaSelectEditar').select2({
        placeholder: 'Seleccionar...',
        dropdownParent: modalActivo ? $('#' + modalActivo) : $('body'),
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    term: params.term || ''
                };
            },
            processResults: function (data) {
                return {
                    results: data.data.map(function (item) {
                        return {
                            id: item.id,
                            text: item.nombre
                        };
                    })
                };
            },
            cache: true
        },
        minimumInputLength: 0
    });

    if (modalActivo === 'modalEditar') {
        const idSelect = '#cuentaSelectEditar';
        const text = document.getElementById('editarSelectCuenta').value;
        buscarSeleccionarSelect2PorTexto(url, idSelect, text);
        document.getElementById('editarSelectCuenta').value = '';
    }
}

// Región: Cargar Select Categoria
function cargarSelect2Categoria(tipoMovimiento) {
    const url = '/categoria/json';
    const tipoMovimientoActual = tipoMovimiento || 'Total';
    var modalActivo = $('.modal.show').attr('id');
    $('#categoriaSelectCrear , #categoriaSelectEditar').select2({
        placeholder: 'Seleccionar...',
        dropdownParent: modalActivo ? $('#' + modalActivo) : $('body'),
        ajax: {
            url: url,
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    term: params.term || '',
                    tipoMovimiento: tipoMovimientoActual
                };
            },
            processResults: function (data) {
                return {
                    results: data.data.map(function (item) {
                        return {
                            id: item.id,
                            text: item.nombre
                        };
                    })
                };
            },
            cache: true
        },
        minimumInputLength: 0
    });

    if (modalActivo === 'modalEditar') {
        const idSelect = '#categoriaSelectEditar';
        const text = document.getElementById('editarSelectCategoria').value;
        buscarSeleccionarSelect2PorTexto(url, idSelect, text);
        document.getElementById('editarSelectCategoria').value = '';
    }
}


