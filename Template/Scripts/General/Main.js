$(document).ready(function () {

    $('.formSpinnerLoad').each(function (index, value) {
        $(this).submit(function () {
            ShowSpinner();
        });
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
