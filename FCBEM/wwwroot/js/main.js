document.addEventListener('DOMContentLoaded', function () {
    // Get the current page URL
    var currentPageUrl = window.location.pathname.toLowerCase();

    // Get all the nav links
    var navLinks = document.querySelectorAll('.nav_link');

    // Remove the "active" class from all nav links
    for (var i = 0; i < navLinks.length; i++) {
        var navLink = navLinks[i];
        navLink.classList.remove('active');
    }

    // Find the nav link that matches the current page URL and add the "active" class
    for (var i = 0; i < navLinks.length; i++) {
        var navLink = navLinks[i];
        if (navLink.getAttribute('href').toLowerCase() === currentPageUrl) {
            navLink.classList.add('active');
            break;
        }
    }
});