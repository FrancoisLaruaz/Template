$(document).ready(function () {


    $('#LinkLogOff').on('click', function () {
        document.getElementById('logoutForm').submit();
    });

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

function LogOffBegin() {
    ShowSpinner();
}

function LogOffSuccess() {

    NotificationOK("[[[See you soon !]]]");
    HideSpinner();
    window.location.href = GetHomePageUrl();
}


function LogOffFailure() {
    ErrorActions();
    window.location.href = GetHomePageUrl();
}

function RefreshHeader() {

    $.ajax({
        url: "/Home/_Header",
        success: function (data) {
            if (data == null) {
                ErrorActions();
            }
            else {
                $("#divHeader").html(data);
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}
