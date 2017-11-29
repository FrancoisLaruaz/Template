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
    SetSignUpSubmitForm();
    if (Data) {
        $('#ErrorSignUpForm').html(Data.Error);
        if (Data.Result) {
          //  RefreshHeader();
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
    $('#SubmitButtonSignUp').val("[[[Create]]]");
    $('#SubmitButtonSignUp').removeAttr('disabled');
    $("#SubmitButtonSignUp").toggleClass("disabled", false);

}


function SetSignUpFormLinks() {
    $("#LoginNowLink").unbind("click");
    $("#LoginNowLink").on("click", function (e) {
        e.preventDefault();    
        if ($("#loginOrSignInModalBody #SignUpForm").length > 0) {
            $("#loginOrSignInModal").modal('show');
            $("#loginOrSignInModalBody #SignUpForm").fadeOut(500, function () {
                $("#LoginForm").fadeIn(500);
            });
        }

    });
}