$(document).ready(function () {
    SetSearchBarOnKeyPressed();
});

function SetSearchBarOnKeyPressed() {
    $(".btn-extended").unbind("change");
    $(".btn-extended").on("change", function (e) {
        var pattern = $(this).val();
        $(".btn-extended").val(pattern);
    });
}

function SearchIconOnClick(e) {
    var searchBar = $(e).parent();
    $(searchBar).toggleClass('clicked');
    var stage = $(e).parent().parent().parent();
    $(stage).toggleClass('faded');
    var btnextended = $(searchBar).find('.btn-extended');
    if ($(searchBar).hasClass('clicked')) {
        $(btnextended).focus();
    }
}