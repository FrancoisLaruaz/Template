$(document).ready(function () {
    SetLoginForm();
    HideSpinner();
});


function LoginFailure()
{
    ErrorActions();
}

function LoginSuccess()
{
    NotificationOK("You are now connected :)");
}

function SetLoginForm()
{
    $('#SubmitButton').show();
}