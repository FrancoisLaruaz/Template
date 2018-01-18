function externalAuthentificationCallback(success, returnUrl, error, media, isSignUp, imageSrc, language) {



    if (isSignUp == 'true') {

        $('#SignUpV2Result').html('');
        $('.resultExternalAuthentification').html('');
        if (success == 'true') {
            hideAndShowGuidePg('loginOrSignInModalBody', 'SignUpWelcomePage');
        } else {
            SetExternalSignUpForm(media);
            document.getElementById("SignUpV2Result_" + media).innerHTML = error;
            HideSpinner();
        }
    }
    else {
        $('#loginV2Result').html('');
        $('.resultExternalAuthentification').html('');
        if (success == 'true') {
            var toGo = $('#CentralGoToUrl').val();


            if (toGo.length > 0 && toGo.trim() != "/") {


                if (toGo.indexOf("http") == -1 && toGo.indexOf("www.") == -1) {

                    if (language != null && language != "") {
                        toGo = "/" + language + toGo;
                    }
                    var Base = GetHomePageUrl();

                    toGo = Base + toGo;
                }

                window.location.href = toGo;
            } else {
                location.reload();
            }
        } else {
            SetExternalLogInForm(media);
            document.getElementById("LogInResult_" + media).innerHTML = error;
            HideSpinner();
        }
    }



}

function invokeExternalAuthentification(IdPopUp) {
    var chrome = 100;
    var width = 500;
    var height = 500;
    var left = (screen.width - width) / 2;
    var top = (screen.height - height - chrome) / 2;
    var options = "status=0,toolbar=0,location=1,resizable=1,scrollbars=1,left=" + left + ",top=" + top + ",width=" + width + ",height=" + height;
    var NewWindow = window.open("about:blank", IdPopUp, options);
    var timer = setInterval(function () {
        if (NewWindow.closed) {
            clearInterval(timer);
            externalAuthentificationWindowClose();
        }
    }, 500);

}


function externalAuthentificationWindowClose() {
    SetExternalSignUpBtns();
    SetExternalLogInBtns();
    HideSpinner();
}

function ExternalSignUpFormOnBegin(Media) {
    $("#HiddenSpinner").fadeIn();
    document.getElementById("SignUpSpan_" + Media).innerHTML = "[[[Signing Up ...]]]";
}

function SetExternalSignUpForm(Media) {
    document.getElementById("SignUpSpan_" + Media).innerHTML = "[[[Sign Up With ]]]" + Media;
}

function SetExternalSignUpBtns() {
    $('.SignUpBtn_js').each(function (index, value) {
        var Provider = $(this).attr("value");
        document.getElementById("SignUpSpan_" + Provider).innerHTML = "[[[Sign Up With ]]]" + Provider;
    });

}

function SetExternalLogInBtns() {


    $('.LogInBtn_js').each(function (index, value) {
        var Provider = $(this).attr("value");
        document.getElementById("LogInSpan_" + Provider).innerHTML = "[[[Log In With ]]]" + Provider;
    });

}

function ExternalLogInFormOnBegin(Media) {
    $("#HiddenSpinner").fadeIn();
    document.getElementById("LogInSpan_" + Media).innerHTML = "[[[Logging In ...]]]";
}

function SetExternalLogInForm(Media) {
    document.getElementById("LogInSpan_" + Media).innerHTML = "[[[Log In With ]]]" + Media;
}