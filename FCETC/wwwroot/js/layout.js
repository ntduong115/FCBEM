let hiddenHeader = false;
function showImages() {
    $('img').each(function () {
        var windowHeight = jQuery(window).height();
        var thisPos = $(this).offset().top;
        var topOfWindow = $(window).scrollTop();
        if (topOfWindow + windowHeight - 20 > thisPos) {
            if (!$(this).hasClass('fadeIn')) {
                $(this).addClass("fadeIn");
            }
        }
        if ($(this).attr('src') === undefined) {
            $(this).attr('src', '/images/no_thumb_article.webp');
        }
    });
};
$(window).scroll(function () {
    showImages();
});
$(document).ready(function () {
    showImages();
    //Check to see if the window is top if not then display button
    $(window).scroll(function () {
        if (hiddenHeader) {
            if ($(this).scrollTop() > 20) {
                $('#header').fadeIn();
            } else {
                $('#header').fadeOut();
            }
        } else {
            $('#header').fadeIn();
        }

        if ($(this).scrollTop() > 80) {
            $('#scrollTop').fadeIn();
        } else {
            $('#scrollTop').fadeOut();
        }
    });
    //Click event to scroll to top
    $('#scrollTop').click(function () {
        $('html, body').animate({ scrollTop: 0 }, 500);
        return false;
    });
    $('#btn-scroll-down').on('click', function (e) {
        e.preventDefault();
        $('html, body').animate({
            scrollTop: $('#next-slide').offset().top
        }, 1000);
        return false;
    });

    if (hiddenHeader) {
        if ($(this).scrollTop() > 20) {
            $('#header').fadeIn();
        } else {
            $('#header').fadeOut();
        }
    } else {
        $('#header').fadeIn();
    }
});

