$(document).ready(function () {
    SetEditNewsForm();
});


function SetEditNewsForm()
{
    SetEnterKey("SubmitNews");
    $('#PublishDate').datetimepicker({
        format: 'MM/DD/YYYY hh:mm:ss',
        defaultDate: new Date()
    });
    SetGenericAjaxForm('EditNewsForm', EditNewsSuccess, EditNewsFailure, null);


}

var EditNewsFailure=function EditNewsFailure() {
    ErrorActions();
}

var EditNewsSuccess = function EditNewsSuccess(data) {

    if (data != null && data.Result) {
        NotificationOK();
        HideSpinner();
    }
    else {
        ErrorActions();
    }
}
