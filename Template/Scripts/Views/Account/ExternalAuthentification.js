function externalAuthentificationCallback(success, returnUrl, error, media, isSignUp, imageSrc, language) {

 

    if (isSignUp == 'true') {

        $('#ErrorSignUpForm').html('');
        $('.resultExternalAuthentification').html('');
        alert('success : ' + success);
        if (success == 'true') {
            hideAndShowGuidePg('loginOrSignInModalBody', 'SignUpWelcomePage');
        } else {
            SetExternalSignUpForm(media);
            document.getElementById("SignUpV2Result_" + media).innerHTML = error;
            HideSpinner();
        }
    }
    else {
        ShowSpinner();
        $('#ErrorLoginForm').html('');
        $('.resultExternalAuthentification').html('');
        alert('success : ' + success);
        if (success == 'true') {
            var toGo = $('#URLRedirect').val();

            alert('toGo : ' + toGo);
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
    ShowSpinner();
}

function SetExternalSignUpForm(Media) {
    
}

function SetExternalSignUpBtns() {
    $('.SignUpBtn_js').each(function (index, value) {
        var Provider = $(this).attr("value");
        document.getElementById("SignUpSpan_" + Provider).innerHTML =  Provider;
    });

}

function SetExternalLogInBtns() {


    $('.LogInBtn_js').each(function (index, value) {
        var Provider = $(this).attr("value");
        document.getElementById("LogInSpan_" + Provider).innerHTML =  Provider;
    });

}

function ExternalLogInFormOnBegin(Media) {
    ShowSpinner();
   // document.getElementById("LogInSpan_" + Media).innerHTML = "[[[Logging In ...]]]";
}

function SetExternalLogInForm(Media) {
    document.getElementById("LogInSpan_" + Media).innerHTML =  Media;
}