$(document).ready(function () {
    $('ul.nav li.dropdown').hover(function () {
        if ($(window).width() > 1070) {
            $(this).addClass('open');
        }
    }, function () {
        if ($(window).width() > 1070) {
            $(this).removeClass('open');
        }
    });

    $('li.dropdown :first-child').on('click', function () {
        var $el = $(this).parent();
        if ($el.hasClass('open')) {
            var $a = $el.children('a.dropdown-toggle');
            if ($a.length && $a.attr('href')) {
                location.href = $a.attr('href');
            }
        }
    });
});