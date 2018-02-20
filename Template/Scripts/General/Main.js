var LoggedUserName = "";


$(document).ready(function () {
    DisplayBrowserBanner();


    var LoggedUserName = $("#HiddenLoggedUserName").val();

    $('.formSpinnerLoad').each(function (index, value) {
        $(this).submit(function () {
            if ($(this).valid()) {
                ShowSpinner();
            }
        });
    });

    $(".BackToHomePageButton_js").unbind("click");
    $(".BackToHomePageButton_js").on("click", function (e) { e.preventDefault(); GoBackToHomePage(); });

    



    //Reset the validation forms of the page
    $('form').each(function (index, value) {
        SetValidationForm($(this).attr('id'));
    });



    SetDateTimeFields();

    $(document)
        .ajaxStart(function () {
            ShowSpinner();
        })
        .ajaxStop(function () {
            HideSpinner();
        });

    if ($("#loginOrSignInModal").length > 0) {
        if (typeof (SetLoginForm) === "function") {
            SetLoginForm();
        }
        if ($("#SignUpForm").length > 0) {
            if (typeof (SetSignUpForm) === "function") {
                SetSignUpForm();
            }
        }
    }

});




function ShowSignUpFormNow() {
    if ($("#loginOrSignInModalBody").length > 0) {

        $("#loginOrSignInModal").modal('show');
        $("#loginOrSignInModalBody #LoginForm").hide();
        $("#loginOrSignInModalBody #SignUpForm").show();
        if ($("#loginOrSignInModalBody #LoginForm").length > 0) {
            $("#div_SignUpFormLinks").show();
           // setTimeout(function () { SetPasswordForm(); }, 1000);
          //  SetPasswordForm();
        }
        else {
            $("#div_SignUpFormLinks").hide();
        }
    }
}