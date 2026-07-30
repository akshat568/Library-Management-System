/* ==========================================================================
   LIBRARY MANAGEMENT SYSTEM - APPLICATION SCRIPTS
   ========================================================================== */

document.addEventListener("DOMContentLoaded", function () {
    // 1. Mobile Sidebar Toggle
    const sidebar = document.getElementById("sidebar");
    const sidebarToggle = document.getElementById("sidebarToggle");
    const sidebarOverlay = document.getElementById("sidebar-overlay");

    if (sidebarToggle && sidebar) {
        sidebarToggle.addEventListener("click", function () {
            sidebar.classList.toggle("show");
            if (sidebarOverlay) {
                sidebarOverlay.classList.toggle("show");
            }
        });
    }

    if (sidebarOverlay) {
        sidebarOverlay.addEventListener("click", function () {
            if (sidebar) sidebar.classList.remove("show");
            sidebarOverlay.classList.remove("show");
        });
    }

    // 2. Initialize Bootstrap Tooltips if Bootstrap bundle is loaded
    if (typeof bootstrap !== "undefined" && bootstrap.Tooltip) {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });
    }

    // 3. Table Row Hover Animation / Ripple
    const tableRows = document.querySelectorAll(".table-hover tbody tr");
    tableRows.forEach(row => {
        row.style.cursor = "pointer";
    });
});
