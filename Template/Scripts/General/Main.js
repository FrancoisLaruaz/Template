$(document).ready(function () {

    $('.formSpinnerLoad').each(function (index, value) {
        $(this).submit(function () {
            ShowSpinner();
        });
    });


    //Reset the validation forms of the page
    $('form').each(function (index, value) {
        SetValidationForm($(this).attr('id'));
    });


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
