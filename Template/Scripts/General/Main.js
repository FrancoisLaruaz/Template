$(document).ready(function () {

    $('.formSpinnerLoad').each(function (index, value) {
        $(this).submit(function () {
            ShowSpinner();
        });
    });


    $(document)
        .ajaxStart(function () {
            ShowSpinner();
        })
        .ajaxStop(function () {
            HideSpinner();
        });

});
