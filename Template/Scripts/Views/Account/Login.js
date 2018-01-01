$(document).ready(function () {

    $("#LoginDiv2 .SignUpLink").unbind("click");
    $("#LoginDiv2 .SignUpLink").on("click", function (e) {
        e.preventDefault();
        ShowSignUpFormNow();
    });

    HideSpinner();
});