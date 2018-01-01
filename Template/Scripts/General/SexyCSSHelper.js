$(document).ready(function () {
    SetStyledCheckboxes();
    SetSexyLabelsForTB();
    SetSexyTB();
    SetSexySelect();
});

function SetSexySelect()
{
    $('.select-text').each(function (index, value) {
        var FieldToAdd = $(this).parent();
        $(FieldToAdd).append('<span class="select-highlight"></span><span class="select-bar"></span >');
    });
}

function SetSexyTB() {

    $('.sexyTB').each(function (index, value) {

        var parentElement = document.querySelector("#" + $(this).attr('id')).parentNode;
        parentElement.children[1].insertAdjacentHTML("afterEnd", '<span class="bar"></span><span class="highlight"></span >');
    });


}


function SetSexyLabelsForTB() {
    $('.sexyTB').each(function (index, value) {
        var Id = $(this).attr('id');
        setSexyTbLabel(Id);

        $(this).blur(function (element, value) {
            $('.sexyTB').each(function (index, value) {
                var Id = $(this).attr('id');
                setSexyTbLabel(Id);
            });
        });


    });
}

function SetStyledCheckboxes() {
    $('.styled-checkbox').each(function (index, value) {
        if ($(this).is(':checked')) {
            $(this).val("true");
        } else {
            $(this).val("false");
        }
        $(this).on("change", function () {

            if ($(this).is(':checked')) {
                $(this).val("true");
            } else {
                $(this).val("false");
            }
        });
    });
}


function setSexyTbLabel(Id) {
    var element = $("#" + Id);
    tmpval = $(element).val();

    if (tmpval == '' || tmpval == null) {
        $(element).addClass('empty');
        $(element).removeClass('not-empty');
    } else {
        $(element).addClass('not-empty');
        $(element).removeClass('empty');
    }
}
