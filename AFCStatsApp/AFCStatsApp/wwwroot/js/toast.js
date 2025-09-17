function showToast(message, type = 'info', duration = 3000) {
    const colors = {
        success: 'bg-success text-white',
        error: 'bg-danger text-white',
        warning: 'bg-warning text-dark',
        info: 'bg-info text-dark'
    };

    const toastEl = document.createElement('div');
    toastEl.innerHTML = `
        <div class="toast position-fixed top-0 start-50 translate-middle-x mt-3 ${colors[type] || colors.info}" 
             role="alert" aria-live="assertive" aria-atomic="true" style="z-index:9999;">
            <div class="toast-body">${message}</div>
        </div>
    `;

    document.body.appendChild(toastEl);
    const bsToast = new bootstrap.Toast(toastEl.querySelector('.toast'), { delay: duration });
    bsToast.show();

    bsToast._element.addEventListener('hidden.bs.toast', () => toastEl.remove());
}

// Expose globally if needed
window.showToast = showToast;
