$(document).ready(function () {
    $("#VideoHomePageDiv").show();

    $("#StartNowHomePage").unbind("click");
    $("#StartNowHomePage").on("click", function (e) {
        e.preventDefault();
        ShowSignUpFormNow();
    });

    HideSpinner();
});


function ShowSignUpFormNow() {
    if ($("#loginOrSignInModalBody").length > 0) {

        $("#loginOrSignInModal").modal('show');
        $("#loginOrSignInModalBody #LoginForm").hide();
        $("#loginOrSignInModalBody #SignUpForm").show();
        if ($("#loginOrSignInModalBody #LoginForm").length > 0) {
            $("#div_SignUpFormLinks").show();
            setTimeout(function () { SetPasswordForm(); }, 1000);
            SetPasswordForm();
        }
        else {
            $("#div_SignUpFormLinks").hide();
        }
    }
}