$(document).ready(function () {

    $("#StartNowHomePage").unbind("click");
    $("#StartNowHomePage").on("click", function (e) {
        e.preventDefault();
        ShowSignUpFormNow();
    });

    HideSpinner();


});


