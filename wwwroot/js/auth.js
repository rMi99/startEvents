// StarEvents Authentication JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Add loading states to form submissions
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', function() {
            const submitBtn = form.querySelector('button[type="submit"]');
            if (submitBtn) {
                submitBtn.classList.add('loading');
                submitBtn.disabled = true;
                
                // Re-enable button after 10 seconds as fallback
                setTimeout(() => {
                    submitBtn.classList.remove('loading');
                    submitBtn.disabled = false;
                }, 10000);
            }
        });
    });

    // Enhanced form validation
    const inputs = document.querySelectorAll('.form-control');
    inputs.forEach(input => {
        input.addEventListener('blur', validateInput);
        input.addEventListener('input', clearErrors);
    });

    function validateInput(e) {
        const input = e.target;
        const value = input.value.trim();
        
        // Email validation
        if (input.type === 'email' && value) {
            const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(value)) {
                showInputError(input, 'Please enter a valid email address');
            }
        }
        
        // Password validation
        if (input.type === 'password' && input.name.includes('Password') && !input.name.includes('Confirm') && value) {
            const minLength = 6;
            if (value.length < minLength) {
                showInputError(input, `Password must be at least ${minLength} characters long`);
            }
        }
        
        // Confirm password validation
        if (input.name.includes('ConfirmPassword') && value) {
            const passwordInput = document.querySelector('input[name*="Password"]:not([name*="Confirm"])');
            if (passwordInput && value !== passwordInput.value) {
                showInputError(input, 'Passwords do not match');
            }
        }
    }

    function clearErrors(e) {
        const input = e.target;
        input.classList.remove('is-invalid');
        const errorMsg = input.parentNode.querySelector('.custom-error');
        if (errorMsg) {
            errorMsg.remove();
        }
    }

    function showInputError(input, message) {
        input.classList.add('is-invalid');
        
        // Remove existing custom error
        const existingError = input.parentNode.querySelector('.custom-error');
        if (existingError) {
            existingError.remove();
        }
        
        // Add new error message
        const errorDiv = document.createElement('div');
        errorDiv.className = 'custom-error text-danger';
        errorDiv.style.fontSize = '0.875rem';
        errorDiv.style.marginTop = '0.25rem';
        errorDiv.textContent = message;
        input.parentNode.appendChild(errorDiv);
    }

    // Social login button animations
    const socialBtns = document.querySelectorAll('.social-btn');
    socialBtns.forEach(btn => {
        btn.addEventListener('mouseenter', function() {
            this.style.transform = 'translateY(-2px) scale(1.02)';
        });
        
        btn.addEventListener('mouseleave', function() {
            this.style.transform = 'translateY(0) scale(1)';
        });
    });

    // Terms checkbox validation
    const termsCheckbox = document.getElementById('terms');
    const registerForm = document.getElementById('registerForm');
    
    if (termsCheckbox && registerForm) {
        registerForm.addEventListener('submit', function(e) {
            if (!termsCheckbox.checked) {
                e.preventDefault();
                
                // Highlight the terms checkbox
                termsCheckbox.style.outline = '2px solid #dc3545';
                termsCheckbox.style.outlineOffset = '2px';
                
                // Show error message
                const errorMsg = document.createElement('div');
                errorMsg.className = 'text-danger mt-2';
                errorMsg.textContent = 'You must agree to the terms and conditions';
                
                if (!termsCheckbox.parentNode.querySelector('.text-danger')) {
                    termsCheckbox.parentNode.appendChild(errorMsg);
                }
                
                // Scroll to checkbox
                termsCheckbox.scrollIntoView({ behavior: 'smooth', block: 'center' });
                
                // Remove highlight after 3 seconds
                setTimeout(() => {
                    termsCheckbox.style.outline = '';
                    termsCheckbox.style.outlineOffset = '';
                }, 3000);
            }
        });
        
        // Remove error when checkbox is checked
        termsCheckbox.addEventListener('change', function() {
            if (this.checked) {
                this.style.outline = '';
                this.style.outlineOffset = '';
                const errorMsg = this.parentNode.querySelector('.text-danger');
                if (errorMsg) {
                    errorMsg.remove();
                }
            }
        });
    }

    // Floating label animation fix
    const floatingInputs = document.querySelectorAll('.form-floating .form-control');
    floatingInputs.forEach(input => {
        // Check if input has value on page load
        if (input.value.trim() !== '') {
            input.classList.add('has-value');
        }
        
        input.addEventListener('blur', function() {
            if (this.value.trim() !== '') {
                this.classList.add('has-value');
            } else {
                this.classList.remove('has-value');
            }
        });
    });

    // Add smooth transitions to form elements
    const formElements = document.querySelectorAll('.form-control, .btn, .form-check-input');
    formElements.forEach(element => {
        element.style.transition = 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)';
    });

    // Add ripple effect to buttons
    const buttons = document.querySelectorAll('.btn');
    buttons.forEach(button => {
        button.addEventListener('click', function(e) {
            const ripple = document.createElement('span');
            const rect = this.getBoundingClientRect();
            const size = Math.max(rect.width, rect.height);
            const x = e.clientX - rect.left - size / 2;
            const y = e.clientY - rect.top - size / 2;
            
            ripple.style.cssText = `
                position: absolute;
                width: ${size}px;
                height: ${size}px;
                left: ${x}px;
                top: ${y}px;
                background: rgba(255, 255, 255, 0.3);
                border-radius: 50%;
                transform: scale(0);
                animation: ripple 0.6s linear;
                pointer-events: none;
            `;
            
            this.style.position = 'relative';
            this.style.overflow = 'hidden';
            this.appendChild(ripple);
            
            setTimeout(() => {
                ripple.remove();
            }, 600);
        });
    });

    // Add CSS for ripple animation
    const style = document.createElement('style');
    style.textContent = `
        @keyframes ripple {
            to {
                transform: scale(2);
                opacity: 0;
            }
        }
        
        .form-floating .form-control.has-value ~ label {
            opacity: 0.65;
            transform: scale(0.85) translateY(-0.5rem) translateX(0.15rem);
        }
        
        .btn.loading {
            color: transparent !important;
        }
        
        .btn.loading::after {
            content: '';
            position: absolute;
            width: 16px;
            height: 16px;
            top: 50%;
            left: 50%;
            margin-left: -8px;
            margin-top: -8px;
            border: 2px solid #ffffff;
            border-top-color: transparent;
            border-radius: 50%;
            animation: spin 1s linear infinite;
        }
        
        @keyframes spin {
            from { transform: rotate(0deg); }
            to { transform: rotate(360deg); }
        }
    `;
    document.head.appendChild(style);

    // Auto-hide validation messages after 5 seconds
    setTimeout(() => {
        const validationMessages = document.querySelectorAll('.text-danger');
        validationMessages.forEach(msg => {
            if (msg.textContent.trim() && !msg.classList.contains('custom-error')) {
                msg.style.transition = 'opacity 0.5s ease';
                msg.style.opacity = '0.7';
            }
        });
    }, 5000);
});
