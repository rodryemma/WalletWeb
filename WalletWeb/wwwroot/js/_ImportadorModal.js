// Variables globales
let selectedFile = null;
const API_BASE_URL = '/excel';

// Referencias a modals
const abrirImportModal = new bootstrap.Modal(document.getElementById('abrirImportModal'));
const progressModal = new bootstrap.Modal(document.getElementById('progressModal'));
const resultsModal = new bootstrap.Modal(document.getElementById('resultsModal'));
const errorModal = new bootstrap.Modal(document.getElementById('errorModal'));

// Referencias a elementos
const elements = {
    abrirImport: document.getElementById('abrirImportBtn'),
    fileInput: document.getElementById('excelFileInput'),
    selectBtn: document.getElementById('selectFileBtn'),
    uploadZone: document.getElementById('uploadZone'),
    fileInfoAlert: document.getElementById('fileInfoAlert'),
    fileName: document.getElementById('fileName'),
    fileDetails: document.getElementById('fileDetails'),
    cancelBtn: document.getElementById('cancelBtn'),
    modalFileName: document.getElementById('modalFileName'),
    modalFileSize: document.getElementById('modalFileSize'),
    modalConfirmBtn: document.getElementById('modalConfirmBtn'),
    modalCancelBtn: document.getElementById('modalCancelBtn'),
    progressBar: document.getElementById('progressBar'),
    progressText: document.getElementById('progressText')
};

// Event Listeners
document.addEventListener('DOMContentLoaded', initializeEventListeners);

function initializeEventListeners() {
    // Eventos de selección de archivo
    elements.selectBtn.addEventListener('click', () => elements.fileInput.click());
    elements.fileInput.addEventListener('change', handleFileSelection);

    // Eventos de botones
    elements.abrirImport.addEventListener('click', showAbrirImportModal);
    elements.cancelBtn.addEventListener('click', resetForm);
    elements.modalConfirmBtn.addEventListener('click', handleImportConfirmation);

    // Drag & Drop
    elements.uploadZone.addEventListener('dragover', handleDragOver);
    elements.uploadZone.addEventListener('dragleave', handleDragLeave);
    elements.uploadZone.addEventListener('drop', handleDrop);
}

// Drag & Drop handlers
function handleDragOver(e) {
    e.preventDefault();
    elements.uploadZone.classList.add('dragover');
}

function handleDragLeave(e) {
    e.preventDefault();
    elements.uploadZone.classList.remove('dragover');
}

function handleDrop(e) {
    e.preventDefault();
    elements.uploadZone.classList.remove('dragover');

    const files = e.dataTransfer.files;
    if (files.length > 0) {
        const file = files[0];
        if (isValidExcelFile(file)) {
            elements.fileInput.files = files;
            handleFileSelection({ target: { files: files } });
        } else {
            showErrorModal('Por favor arrastra un archivo Excel válido (.xlsx o .xls)');
        }
    }
}

// Manejar selección de archivo
function handleFileSelection(event) {
    const file = event.target.files[0];

    if (!file) {
        resetForm();
        return;
    }

    if (!isValidExcelFile(file)) {
        showErrorModal('Por favor selecciona un archivo Excel válido (.xlsx o .xls)');
        resetForm();
        return;
    }

    if (file.size > 10 * 1024 * 1024) {
        showErrorModal('El archivo es demasiado grande. Tamaño máximo: 10MB');
        resetForm();
        return;
    }

    selectedFile = file;
    showFileInfo(file);
}

// Validar archivo Excel
function isValidExcelFile(file) {
    const validTypes = [
        'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        'application/vnd.ms-excel'
    ];
    const validExtensions = ['.xlsx', '.xls'];
    const fileName = file.name.toLowerCase();

    return validTypes.includes(file.type) ||
        validExtensions.some(ext => fileName.endsWith(ext));
}

// Mostrar modal de seleccionar archivo
function showAbrirImportModal() {
    abrirImportModal.show();
}

// Mostrar información del archivo
function showFileInfo(file) {
    elements.fileName.textContent = file.name;
    elements.fileDetails.textContent = `${formatFileSize(file.size)} • Excel`;
    elements.fileInfoAlert.classList.remove('d-none');
    elements.fileInfoAlert.classList.add('fade-in');
}


// Manejar confirmación de importación
async function handleImportConfirmation() {
    if (!selectedFile) return;

    abrirImportModal.hide();

    try {
        // Mostrar modal de progreso
        progressModal.show();
        simulateProgress();

        // Crear FormData
        const formData = new FormData();
        formData.append('excelFile', selectedFile);

        // Realizar petición
        const response = await fetch(`${API_BASE_URL}/import`, {
            method: 'POST',
            body: formData
        });

        const result = await response.json();

        // Ocultar modal de progreso
        progressModal.hide();

        if (result.success) {
            showResultsModal(result);
        } else {
            showErrorModal(result.message || 'Error al importar el archivo');
        }

    } catch (error) {
        console.error('Error durante importación:', error);
        progressModal.hide();
        showErrorModal('Error de conexión. Por favor intenta nuevamente.');
    } finally {
        resetForm();
    }
}

// Simular progreso
function simulateProgress() {
    let progress = 0;
    const steps = [
        { progress: 25, text: 'Validando archivo...' },
        { progress: 50, text: 'Leyendo datos...' },
        { progress: 75, text: 'Procesando transacciones...' },
        { progress: 90, text: 'Guardando en base de datos...' },
        { progress: 100, text: 'Completado' }
    ];

    function updateProgress() {
        if (progress < steps.length) {
            const step = steps[progress];
            elements.progressBar.style.width = `${step.progress}%`;
            elements.progressText.textContent = step.text;
            progress++;
            setTimeout(updateProgress, 600);
        }
    }

    updateProgress();
}

// Mostrar modal de resultados
function showResultsModal(result) {
    const data = result.data || {};

    // Actualizar estadísticas
    document.getElementById('totalRows').textContent = data.totalRows || 0;
    document.getElementById('processedRows').textContent = data.processedRows || 0;
    document.getElementById('errorRows').textContent = data.errorRows || 0;

    const successRate = data.totalRows > 0 ?
        Math.round((data.processedRows / data.totalRows) * 100) : 0;
    document.getElementById('successRate').textContent = `${successRate}%`;

    // Mensaje de resultado
    document.getElementById('resultMessage').textContent = result.message;

    // Mostrar errores si existen
    if (data.errors && data.errors.length > 0) {
        const errorSection = document.getElementById('errorSection');
        const errorList = document.getElementById('errorList');

        errorList.innerHTML = data.errors.map((error, index) =>
            `<div class="mb-2">
                    <span class="badge bg-danger me-2">${index + 1}</span>${error}
                </div>`
        ).join('');

        errorSection.classList.remove('d-none');
    }

    resultsModal.show();
}

// Mostrar modal de error
function showErrorModal(message) {
    document.getElementById('errorMessage').textContent = message;
    errorModal.show();
}

// Resetear formulario
function resetForm() {
    selectedFile = null;
    elements.fileInput.value = '';
    elements.fileInfoAlert.classList.add('d-none');
    elements.uploadZone.classList.remove('dragover');
}

// Formatear tamaño de archivo
function formatFileSize(bytes) {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
}