$(document).ready(function () {
    SetSignUpForm();
    SetSignUpFormLinks();
});



function SignUpFailure()
{
    SetSignUpSubmitForm();
    ErrorActions();
}

function handleSignUpBegin()
{
    $('#SubmitButtonSignUp').val("[[[Creation ...]]]");
    $("#SubmitButtonSignUp").toggleClass("disabled", true);
}

function SignUpSuccess(Data)
{
    if (Data) {
        $('#ErrorSignUpForm').html(Data.Error);
        if (Data.Result) {
            hideAndShowGuidePg('loginOrSignInModalBody', 'SignUpWelcomePage');
          //  showGuidePg('SignUpProcessPages');
          //  RefreshHeader();
        }
        else {
            SetSignUpSubmitForm();
        }
    }
    else {
        SetSignUpSubmitForm();
    }
}

function SetSignUpForm()
{
    $("#SignUpWithEmail").unbind("click");
    $("#SignUpWithEmail").on("click", function (e) {
        e.preventDefault();
        $("#SignUpChoice").fadeOut(500, function () {
            $("#SignUpFormDiv").fadeIn(500);
        });
    });


    $("#GoBackToSignUpChoiceBtn").unbind("click");
    $("#GoBackToSignUpChoiceBtn").on("click", function (e) {
        e.preventDefault();
        $("#SignUpFormDiv").fadeOut(500, function () {
            $("#SignUpChoice").fadeIn(500);
        });
    });
    SetEnterKey('SubmitButtonSignUp'); 
    SetSignUpSubmitForm();
    //SetValidationForm('SignUpModalForm');
}

function SetSignUpSubmitForm()
{
    $('#SubmitButtonSignUp').show();
    $('#SubmitButtonSignUp').val("[[[Create]]]");
    $('#SubmitButtonSignUp').removeAttr('disabled');
    $("#SubmitButtonSignUp").toggleClass("disabled", false);

}


function SetSignUpFormLinks() {
    $("#LoginNowLink").unbind("click");
    $("#LoginNowLink").on("click", function (e) {
        e.preventDefault();    
        ShowLogInForm();
    });
}