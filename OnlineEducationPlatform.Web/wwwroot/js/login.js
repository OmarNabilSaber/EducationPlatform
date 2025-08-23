// Modern Login Page JavaScript
document.addEventListener('DOMContentLoaded', function() {
    const loginForm = document.getElementById('loginForm');
    const loginButton = document.querySelector('.btn-login');
    const emailInput = document.getElementById('Email');
    const passwordInput = document.getElementById('Password');

    // Add floating shapes to background
    createFloatingShapes();

    // Form validation and enhancement
    if (loginForm) {
        loginForm.addEventListener('submit', handleFormSubmit);
    }

    // Real-time validation
    if (emailInput) {
        emailInput.addEventListener('blur', validateEmail);
        emailInput.addEventListener('input', clearEmailError);
    }

    if (passwordInput) {
        passwordInput.addEventListener('blur', validatePassword);
        passwordInput.addEventListener('input', clearPasswordError);
    }

    // Add focus effects
    const inputs = document.querySelectorAll('.form-control');
    inputs.forEach(input => {
        input.addEventListener('focus', addFocusEffect);
        input.addEventListener('blur', removeFocusEffect);
    });

    // Add typing animation to title
    animateTitle();
});

function createFloatingShapes() {
    const container = document.querySelector('.login-container');
    if (!container) return;

    const shapesContainer = document.createElement('div');
    shapesContainer.className = 'floating-shapes';
    
    for (let i = 0; i < 3; i++) {
        const shape = document.createElement('div');
        shape.className = 'shape';
        shapesContainer.appendChild(shape);
    }
    
    container.appendChild(shapesContainer);
}

function handleFormSubmit(e) {
    const loginButton = document.querySelector('.btn-login');
    const emailInput = document.getElementById('Email');
    const passwordInput = document.getElementById('Password');

    // Basic validation
    let isValid = true;

    if (!validateEmail()) isValid = false;
    if (!validatePassword()) isValid = false;

    if (!isValid) {
        e.preventDefault();
        return false;
    }

    // Add loading state
    if (loginButton) {
        loginButton.classList.add('loading');
        loginButton.innerHTML = '<i class="bi bi-hourglass-split"></i> Signing In...';
    }

    // Simulate loading delay (remove in production)
    setTimeout(() => {
        if (loginButton) {
            loginButton.classList.remove('loading');
            loginButton.innerHTML = '<i class="bi bi-box-arrow-in-right"></i> Login';
        }
    }, 2000);
}

function validateEmail() {
    const emailInput = document.getElementById('Email');
    const emailErrorSpan = document.querySelector('[data-valmsg-for="Email"]');
    
    if (!emailInput) return true;

    const email = emailInput.value.trim();
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!email) {
        showError(emailInput, emailErrorSpan, 'Email is required');
        return false;
    } else if (!emailRegex.test(email)) {
        showError(emailInput, emailErrorSpan, 'Please enter a valid email address');
        return false;
    } else {
        clearError(emailInput, emailErrorSpan);
        return true;
    }
}

function validatePassword() {
    const passwordInput = document.getElementById('Password');
    const passwordErrorSpan = document.querySelector('[data-valmsg-for="Password"]');
    
    if (!passwordInput) return true;

    const password = passwordInput.value;

    if (!password) {
        showError(passwordInput, passwordErrorSpan, 'Password is required');
        return false;
    } else if (password.length < 6) {
        showError(passwordInput, passwordErrorSpan, 'Password must be at least 6 characters');
        return false;
    } else {
        clearError(passwordInput, passwordErrorSpan);
        return true;
    }
}

function showError(input, errorSpan, message) {
    clearError(input, errorSpan);
    
    if (errorSpan) {
        errorSpan.textContent = message;
        errorSpan.style.display = 'block';
    }
    
    input.style.borderColor = '#e53e3e';
}

function clearError(input, errorSpan) {
    if (errorSpan) {
        errorSpan.textContent = '';
        errorSpan.style.display = 'none';
    }
    input.style.borderColor = '#e2e8f0';
}

function clearEmailError() {
    const emailInput = document.getElementById('Email');
    const emailErrorSpan = document.querySelector('[data-valmsg-for="Email"]');
    if (emailInput) {
        clearError(emailInput, emailErrorSpan);
    }
}

function clearPasswordError() {
    const passwordInput = document.getElementById('Password');
    const passwordErrorSpan = document.querySelector('[data-valmsg-for="Password"]');
    if (passwordInput) {
        clearError(passwordInput, passwordErrorSpan);
    }
}

function addFocusEffect(e) {
    const inputGroup = e.target.closest('.input-group');
    if (inputGroup) {
        inputGroup.style.transform = 'translateY(-2px)';
    }
}

function removeFocusEffect(e) {
    const inputGroup = e.target.closest('.input-group');
    if (inputGroup) {
        inputGroup.style.transform = 'translateY(0)';
    }
}

function animateTitle() {
    const title = document.querySelector('.login-title');
    if (!title) return;

    const text = title.textContent;
    title.textContent = '';
    
    let i = 0;
    const typeWriter = () => {
        if (i < text.length) {
            title.textContent += text.charAt(i);
            i++;
            setTimeout(typeWriter, 100);
        }
    };
    
    // Start typing animation after a short delay
    setTimeout(typeWriter, 500);
}

// Add smooth scroll behavior
document.documentElement.style.scrollBehavior = 'smooth';

// Add keyboard navigation
document.addEventListener('keydown', function(e) {
    if (e.key === 'Enter' && e.target.classList.contains('form-control')) {
        const form = e.target.closest('form');
        if (form) {
            const submitButton = form.querySelector('button[type="submit"]');
            if (submitButton) {
                submitButton.click();
            }
        }
    }
});

// Add accessibility improvements
function enhanceAccessibility() {
    const inputs = document.querySelectorAll('.form-control');
    inputs.forEach(input => {
        if (!input.getAttribute('aria-describedby')) {
            const id = input.id || input.name;
            input.setAttribute('aria-describedby', `${id}-error`);
        }
    });
}

// Initialize accessibility enhancements
document.addEventListener('DOMContentLoaded', enhanceAccessibility);
