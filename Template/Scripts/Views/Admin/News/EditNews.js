$(document).ready(function () {
    SetEditNewsForm();
});


function SetEditNewsForm()
{
    TypeOnChange();
    $("#TypeId").unbind("change");
    $("#TypeId").on("change", function (e) { TypeOnChange(); });

    $("#MailPreview").unbind("click");
    $("#MailPreview").on("click", function (e) { PreviewMailOnClick(); });

    $("#MailSubject").addClass("RequireSubject");
    $.validator.addMethod("RequireSubject",
        function (value, element, options) {
            return !(($("#MailSubject").val().trim().length == 0) && IsMailNeeded())
        }, "[[[Mail Subject is a required field.]]]");
    $.validator.classRuleSettings.RequireSubject = { RequireSubject: true };


    $("#TypeUserMailingId").addClass("RequireTypeUserMailing");
    $.validator.addMethod("RequireTypeUserMailing",
        function (value, element, options) {
            return !(($("#TypeUserMailingId").val() == 0 || $("#TypeUserMailingId").val() == '') && IsMailNeeded())
        }, "[[[Type User Mailing is a required field.]]]");
    $.validator.classRuleSettings.RequireTypeUserMailing = { RequireTypeUserMailing: true };

    SetEnterKey("SubmitNews");
    SetGenericAjaxForm('EditNewsForm', EditNewsSuccess, EditNewsFailure, null);
}

function EditNewsFailure() {
    ErrorActions();
}

function EditNewsSuccess(data) {



    if (data != null && data.Result) {
        if (data.IsCreation) {
            window.location.href = GetHomePageUrl() + '/Admin/News';
        }
        else {
            NotificationOK('[[[The news has been successfully created.]]]');
            BackToTop();
            HideSpinner();
        }
    }
    else {
        ErrorActions();
    }
}


function  PreviewMailOnClick() {
   // $("#spinner").fadeIn();
    alert('PreviewMailOnClick ');
}


function TypeOnChange() {

    if (IsMailNeeded()) {
        $(".RelatedToNewsMail").fadeIn();
    }
    else {
        $(".RelatedToNewsMail").fadeOut();
    }
}


function IsMailNeeded() {
    var result = false;
    var TypeVal = $("#TypeId").val();
    if (TypeVal != null && (TypeVal == Constants.NewsType.PublishAndMail || TypeVal.toString() == Constants.NewsType.MailOnly)) {
        result = true;
    }

    return result;
}