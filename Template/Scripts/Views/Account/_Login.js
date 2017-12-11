$(document).ready(function () {
    SetLoginForm();
    SetLoginFormLinks();
});

function SetLoginFormLinks() {
    $("#PasswordForgotLink").unbind("click");
    $("#PasswordForgotLink").on("click", function (e) {
        e.preventDefault();
        if ($('#loginOrSignInModal').length > 0) {
            $('#loginOrSignInModal').click();
        }
        var newUrl = GetHomePageUrl() + "/ResetPassword";
        window.location.href = newUrl;
    });


    $("#SignUpLink").unbind("click");
    $("#SignUpLink").on("click", function (e) {
        e.preventDefault();
        ShowSignUpForm();
    });
}


function LoginFailure() {
    SetLoginSubmitForm();
    ErrorActions();
}

function handleLoginBegin() {
    $('#SubmitButtonLogin').val("[[[Logging In ...]]]");
    $("#SubmitButtonLogin").toggleClass("disabled", true);
}

function LoginSuccess(Data) {
    SetLoginSubmitForm();
    if (Data) {
        $('#ErrorLoginForm').html(Data.Error);
        if (Data.Result) {
            if ($('#loginOrSignInModal').length > 0) {
                $('#loginOrSignInModal').click();
            }
         
            if (Data.URLRedirect != null && Data.URLRedirect != "") {
                window.location.href = Data.URLRedirect;
            }
            else {
                window.location.href = GetHomePageUrl();
            }
        }
    }
}

function SetLoginForm() {
    if ($("#loginOrSignInModalBody #LoginForm").length > 0) {
        SetEnterKey('SubmitButtonLogin');
        SetLoginSubmitForm();
    }

}

function SetLoginSubmitForm() {
    $('#SubmitButtonLogin').show();
    $('#SubmitButtonLogin').val("[[[Log In]]]");
    $('#SubmitButtonLogin').removeAttr('disabled');
    $("#SubmitButtonLogin").toggleClass("disabled", false);

}