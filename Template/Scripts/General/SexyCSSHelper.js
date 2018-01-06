$(document).ready(function () {
    SetSexyCSS();
});

function SetSexyCSS() {
    SetStyledCheckboxes();
    SetSexyLabelsForTB();
    SetSexyTB();
    SetSexySelect();
}

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
        SetSexyTbLabel(Id);

        $(this).blur(function (element, value) {
            $('.sexyTB').each(function (index, value) {
                var Id = $(this).attr('id');
                SetSexyTbLabel(Id);
            });
        });

        $(this).change(function (element, value) {
            $('.sexyTB').each(function (index, value) {
                var Id = $(this).attr('id');
                SetSexyTbLabel(Id);
            });
        });

    });
}

function SetStyledCheckboxes() {
    $('.styled-checkbox').each(function (index, value) {
        var name = $(this).attr('name');
        var HiddenInput = $('input:not(.styled-checkbox)[name=' + name + ']');


        if (HiddenInput != "undefined" && HiddenInput != null) {
            $(HiddenInput).remove();
            var parentElement = document.querySelector("#" + $(this).attr('id')).parentNode;
            parentElement.children[1].insertAdjacentHTML("afterEnd", ' <input name="' + name + '" value="false" type="hidden">');
        }
    });
}


function SetSexyTbLabel(Id) {
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
