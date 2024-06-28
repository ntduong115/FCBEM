(function () {
    'use strict';

    function calculateMarginValue(input) {
        return input + 'px'; // Add 'px' unit to the margin value
    }


    function updateMarginTop() {
        var div1 = document.getElementById('banner_home');
        var div2 = document.getElementById('header');

        var div2Height = div2.offsetHeight;

        var marginValue = calculateMarginValue(div2Height);

        div1.style.marginTop = marginValue;

        console.log("Updated margin-top:", marginValue);
    }
    // Add 'load' event listener to window
    window.addEventListener('load', function () {
        // Call updateMarginTop function after the page is fully loaded
        updateMarginTop();
    });

    document.addEventListener('DOMContentLoaded', function () {
        const menu_mobile = document.getElementById("header-toggle");
        if (menu_mobile) {
            menu_mobile.addEventListener('click', function () {
                var navMenu = document.getElementById('nav-menu');

                // Check if the nav-menu element has the 'show' class
                if (navMenu.classList.contains('show')) {
                    navMenu.classList.remove('show');
                    menu_mobile.src = 'https://cdn-icons-png.flaticon.com/512/2767/2767162.png';

                } else {
                    navMenu.classList.add('show');
                    menu_mobile.src = 'https://cdn-icons-png.flaticon.com/512/2997/2997911.png';

                }
            });
        }
    });


    // Add 'resize' event listener to window
    window.addEventListener('resize', function () {
        updateMarginTop();
    });

    console.log("The code is running");
})(window.jQuery);