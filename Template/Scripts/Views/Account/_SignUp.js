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
            showGuidePg('SignUpProcess');
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