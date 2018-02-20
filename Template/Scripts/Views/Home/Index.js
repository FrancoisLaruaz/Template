$(document).ready(function () {

    $("#StartNowHomePage").unbind("click");
    $("#StartNowHomePage").on("click", function (e) {
        e.preventDefault();
        ShowSignUpFormNow();
    });

    if ($("#PromptLogin").prop("checked")) {
        recordGoToUrl($("#hidden_RedirectTo").val());
        ShowLogInForm();
    }
    else if ($("#SignUp").prop("checked"))
    {
        ShowSignUpFormNow();
    }

    HideSpinner();


});


