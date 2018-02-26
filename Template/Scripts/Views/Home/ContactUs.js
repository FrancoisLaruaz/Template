$(document).ready(function () {


    HideSpinner();
});

function ContactUsFailure() {
    ErrorActions();
}

function ContactUsSuccess(data) {
    $("#ErrorContactUsForm").html('');

    if (data != null && data.Result) {

        alert('success');
    }
    else if(data.Error!=null && data.Error.trim()!='')
    {
        $("#ErrorContactUsForm").html(data.Error);
    }
    else {
        ContactUsFailure();
    }
}

function handleContactUsBegin() {

}