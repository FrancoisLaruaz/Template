$(document).ready(function () {
    SetSexyCSS();
    SetValidationForm('MyProfileEditModalForm');
    HideSpinner();
});

function MyProfileEditFailure() {
    ErrorActions();
}

function MyProfileEditSuccess(data) {
    $("#ErrorMyProfileEditForm").html('');

    if (data != null && data.Result) {

        NotificationOK('[[[Your profile has been successfully saved.]]]');
    }
    else if (data.Error != null && data.Error.trim() != '') {
        $("#ErrorMyProfileEditForm").html(data.Error);
    }
    else {
        MyProfileEditFailure();
    }
    BackToTop();
}

function handleMyProfileEditBegin() {

}