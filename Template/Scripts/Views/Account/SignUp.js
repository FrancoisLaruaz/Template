$(document).ready(function () {
    SetSignUpForm();
});



function SignUpFailure()
{
    SetSignUpSubmitForm();
    ErrorActions();
}

function handleSignUpBegin()
{
    $('#SubmitButtonSignUp').val("Creation ...");
    $("#SubmitButtonSignUp").toggleClass("disabled", true);
}

function SignUpSuccess(Data)
{
    SetSignUpSubmitForm();
    if (Data) {
        $('#ErrorSignUpForm').html(Data.Error);
        if (Data.Result) {

        }
    }
}

function SetSignUpForm()
{
    SetEnterKey('SubmitButtonSignUp'); 
    SetSignUpSubmitForm();
    SetValidationForm('SignUpModalForm');
}

function SetSignUpSubmitForm()
{
    $('#SubmitButtonSignUp').show();
    $('#SubmitButtonSignUp').val("Create");
    $('#SubmitButtonSignUp').removeAttr('disabled');
    $("#SubmitButtonSignUp").toggleClass("disabled", false);

}