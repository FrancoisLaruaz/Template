$(document).ready(function () {
    SetLoginForm();
});


function LoginFailure()
{
    SetLoginSubmitForm();
    ErrorActions();
}

function handleLoginBegin()
{
    $('#SubmitButton').val("Logging In ...");
    $("#SubmitButton").toggleClass("disabled", true);
}

function LoginSuccess(Data)
{
    SetLoginSubmitForm();
    if (Data) {
        $('#ErrorForm').html(Data.Error);
        if (Data.Result) {
            if (Data.URLRedirect != null && model.URLRedirect != "")
            {
                window.location.href = Data.URLRedirect;
            }

            NotificationOK(Data.UserFirstName+", you are now connected :)");
        }
    }
}

function SetLoginForm()
{
    SetLoginSubmitForm();
    SetValidationForm('LoginModalForm');
}

function SetLoginSubmitForm()
{
    $('#SubmitButton').show();
    $('#SubmitButton').val("Log In");
    $('#SubmitButton').removeAttr('disabled');
    $("#SubmitButton").toggleClass("disabled", false);
}