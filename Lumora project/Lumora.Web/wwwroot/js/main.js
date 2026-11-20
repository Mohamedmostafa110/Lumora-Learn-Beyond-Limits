/**
 * ============================================================================
 * LUMORA — Learn Beyond Limits | Enterprise Design System
 * File: main.js
 * Purpose: Global interactive UI logic, navbar scroll behaviors,
 *          password visibility toggler, role switchers, and utility helpers.
 * Compatible with: ASP.NET Core MVC (Lumora.Web / wwwroot/js)
 * ============================================================================
 */

(function () {
    'use strict';

    // 1. Initialize Bootstrap Tooltips & Popovers
    document.addEventListener('DOMContentLoaded', function () {
        const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.map(function (tooltipTriggerEl) {
            return new bootstrap.Tooltip(tooltipTriggerEl);
        });

        // 2. Navbar Scroll Behavior
        const navbar = document.querySelector('.lumora-navbar');
        if (navbar) {
            window.addEventListener('scroll', function () {
                if (window.scrollY > 40) {
                    navbar.classList.add('navbar-scrolled');
                } else {
                    navbar.classList.remove('navbar-scrolled');
                }
            });
        }

        // 3. Password Visibility Toggler
        const toggleButtons = document.querySelectorAll('.password-toggle-btn');
        toggleButtons.forEach(btn => {
            btn.addEventListener('click', function () {
                const input = this.closest('.password-toggle-wrapper').querySelector('input');
                const icon = this.querySelector('i');
                if (input.type === 'password') {
                    input.type = 'text';
                    if (icon) {
                        icon.classList.remove('bi-eye');
                        icon.classList.add('bi-eye-slash');
                    }
                } else {
                    input.type = 'password';
                    if (icon) {
                        icon.classList.remove('bi-eye-slash');
                        icon.classList.add('bi-eye');
                    }
                }
            });
        });

        // 4. Register Page Role Switcher (Student vs Instructor)
        const roleTabs = document.querySelectorAll('.auth-role-tab');
        const instructorFields = document.getElementById('instructorFields');
        const roleInput = document.getElementById('selectedRoleInput');

        roleTabs.forEach(tab => {
            tab.addEventListener('click', function () {
                roleTabs.forEach(t => t.classList.remove('active'));
                this.classList.add('active');
                const role = this.getAttribute('data-role');
                if (roleInput) roleInput.value = role;

                if (instructorFields) {
                    if (role === 'instructor') {
                        instructorFields.classList.remove('d-none');
                    } else {
                        instructorFields.classList.add('d-none');
                    }
                }
            });
        });

        // 5. Featured Courses Category Filter on Landing Page
        const filterButtons = document.querySelectorAll('.course-filter-btn');
        const courseCards = document.querySelectorAll('.course-filter-item');

        filterButtons.forEach(btn => {
            btn.addEventListener('click', function () {
                filterButtons.forEach(b => b.classList.remove('active'));
                this.classList.add('active');
                const category = this.getAttribute('data-category');

                courseCards.forEach(card => {
                    if (category === 'all' || card.getAttribute('data-category') === category) {
                        card.style.display = 'block';
                    } else {
                        card.style.display = 'none';
                    }
                });
            });
        });

        // 6. Landing Quick Certificate Lookup Form Handler
        const quickCertForm = document.getElementById('quickCertForm');
        if (quickCertForm) {
            quickCertForm.addEventListener('submit', function (e) {
                e.preventDefault();
                const input = document.getElementById('quickCertInput');
                if (input && input.value.trim() !== '') {
                    const certId = encodeURIComponent(input.value.trim());
                    window.location.href = 'public/verify-certificate.html?certId=' + certId;
                }
            });
        }
    });

    // Global Lumora Alert / Toast Helper
    window.LumoraToast = function (message, type = 'gold') {
        const toastContainer = document.getElementById('lumoraToastContainer');
        if (!toastContainer) {
            const container = document.createElement('div');
            container.id = 'lumoraToastContainer';
            container.style.cssText = 'position:fixed;bottom:24px;right:24px;z-index:9999;display:flex;flex-direction:column;gap:10px;';
            document.body.appendChild(container);
        }
        const toast = document.createElement('div');
        toast.className = 'card shadow-md border-lumora-subtle p-3 animate__animated animate__fadeInUp';
        toast.style.cssText = 'min-width:280px;background:#14243A;color:#FFFFFF;border-left:4px solid #D6B779;';
        toast.innerHTML = `
      <div class="d-flex align-items-center justify-content-between">
        <span class="small font-heading font-semibold"><i class="bi bi-shield-check text-lumora-champagne me-2"></i>${message}</span>
        <button type="button" class="btn-close btn-close-white btn-sm ms-2" onclick="this.parentElement.parentElement.remove()"></button>
      </div>
    `;
        document.getElementById('lumoraToastContainer').appendChild(toast);
        setTimeout(() => {
            toast.remove();
        }, 4000);
    };
})();
