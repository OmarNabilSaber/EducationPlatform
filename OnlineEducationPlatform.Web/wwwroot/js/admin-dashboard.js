// Simple Admin Dashboard JavaScript
document.addEventListener('DOMContentLoaded', function() {
    // Initialize dashboard interactions
    initializeDashboard();
    
    // Add simple animations
    initializeAnimations();
});

function initializeDashboard() {
    // Add click tracking for analytics (optional)
    const cards = document.querySelectorAll('.dashboard-card');
    cards.forEach(card => {
        card.addEventListener('click', function(e) {
            // Only track if clicking on the card itself, not the button
            if (e.target === card || e.target.closest('.card-description')) {
                const title = card.querySelector('.card-title').textContent;
                console.log(`Card clicked: ${title}`);
            }
        });
    });
}

function initializeAnimations() {
    const cards = document.querySelectorAll('.dashboard-card');
    
    cards.forEach((card, index) => {
        // Add staggered animation delay
        card.style.animationDelay = `${index * 0.1}s`;
        
        // Add intersection observer for scroll animations
        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.style.opacity = '1';
                    entry.target.style.transform = 'translateY(0)';
                }
            });
        }, {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        });
        
        observer.observe(card);
    });
}

// Add loading states for card buttons
function addLoadingState(button) {
    const card = button.closest('.dashboard-card');
    if (card) {
        card.classList.add('loading');
        button.innerHTML = '<i class="bi bi-hourglass-split"></i> Loading...';
    }
}

function removeLoadingState(button) {
    const card = button.closest('.dashboard-card');
    if (card) {
        card.classList.remove('loading');
        // Restore original button content
        const originalText = button.getAttribute('data-original-text') || 'Go to';
        const icon = button.getAttribute('data-original-icon') || 'bi-arrow-right';
        button.innerHTML = `<i class="bi ${icon}"></i> ${originalText}`;
    }
}

// Add smooth scrolling for better UX
document.documentElement.style.scrollBehavior = 'smooth';

// Add keyboard navigation
document.addEventListener('keydown', function(e) {
    if (e.key === 'Enter' && e.target.classList.contains('card-button')) {
        e.target.click();
    }
});

// Add accessibility improvements
function enhanceAccessibility() {
    const cards = document.querySelectorAll('.dashboard-card');
    cards.forEach((card, index) => {
        // Add ARIA labels
        const title = card.querySelector('.card-title');
        const description = card.querySelector('.card-description');
        const button = card.querySelector('.card-button');
        
        if (title && button) {
            button.setAttribute('aria-label', `Navigate to ${title.textContent} section`);
        }
        
        if (description) {
            card.setAttribute('aria-describedby', `card-desc-${index}`);
            description.id = `card-desc-${index}`;
        }
    });
}

// Add responsive behavior
function handleResponsiveBehavior() {
    const cards = document.querySelectorAll('.dashboard-card');
    
    function adjustCardLayout() {
        const isMobile = window.innerWidth <= 768;
        
        cards.forEach(card => {
            if (isMobile) {
                card.style.marginBottom = '1rem';
            } else {
                card.style.marginBottom = '0';
            }
        });
    }
    
    // Initial adjustment
    adjustCardLayout();
    
    // Adjust on window resize
    window.addEventListener('resize', adjustCardLayout);
}

// Initialize all features
document.addEventListener('DOMContentLoaded', function() {
    enhanceAccessibility();
    handleResponsiveBehavior();
});

// Add simple card interaction tracking
function trackCardInteractions() {
    const cards = document.querySelectorAll('.dashboard-card');
    
    cards.forEach(card => {
        const title = card.querySelector('.card-title').textContent;
        
        // Track clicks
        card.addEventListener('click', function() {
            console.log(`${title} card clicked`);
        });
    });
}

// Initialize interaction tracking
document.addEventListener('DOMContentLoaded', trackCardInteractions);
