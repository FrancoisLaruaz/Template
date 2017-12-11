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

function ShowSignUpForm() {
    if ($("#loginOrSignInModalBody").length > 0) {

        $("#loginOrSignInModal").modal('show');

        $("#loginOrSignInModalBody #LoginForm").fadeOut(500, function () {
            $("#loginOrSignInModalBody #SignUpForm").fadeIn(500);
            if ($("#loginOrSignInModalBody #LoginForm").length > 0) {
                $("#div_SignUpFormLinks").show();
                setTimeout(function () { SetPasswordForm(); }, 1000);
                SetPasswordForm();
            }
            else {
                $("#div_SignUpFormLinks").hide();
            }
        });
    }
}





function ShowLogInForm() {
    if ($("#loginOrSignInModalBody #SignUpForm").length > 0) {
        $("#loginOrSignInModal").modal('show');
        $("#loginOrSignInModalBody #SignUpForm").fadeOut(500, function () {
            $("#loginOrSignInModalBody #LoginForm").fadeIn(500);
        });
    }
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


function recordGoToUrl(shouldGoTo) {

    if (shouldGoTo == null) {
        $('#CentralGoToUrl').val('@HttpContext.Current.Request.Url.AbsolutePath'); // optional param not passed in, so use current page url
    } else {
        $('#CentralGoToUrl').val(shouldGoTo);
    }
}


function ShowSecretlyLogInForm() {

    if ($("#loginOrSignInModalBody #LoginForm").length > 0) {
        $("#loginOrSignInModalBody #SignUpForm").hide();
        $("#loginOrSignInModalBody #LoginForm").show();
    }
}