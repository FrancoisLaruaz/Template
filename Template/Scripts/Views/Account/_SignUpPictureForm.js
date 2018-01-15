function SetSkipPictureOnClick()
{


    $("#SkipPicture").unbind("click");
    $("#SkipPicture").on("click", function (e) {
        e.preventDefault();
        hideAndShowGuidePg('newUserPicturePg', 'newUserConfirmationPg');
    });
}

function SetSignUpPictureForm()
{
    SetCamera('UserPicture', 'CameraPictureUser', 'PictureSrc', 'CameraPictureUserPreview');
    SetDragAndDropPicture('CameraPictureUserPreview', 'UserPicture', 'PictureSrc','CameraPictureUserPreview')
    SetSkipPictureOnClick();
}

function SignUpPictureSuccess(data)
{
    if (data)
    {
        if (data.Result)
        {
            hideAndShowGuidePg('newUserPicturePg', 'newUserConfirmationPg');
        }
        else {
            ErrorActions();
        }
    }
}


function SignUpPictureFailure() {
    ErrorActions();
}