

function SetDragAndDropEvents()
{
    $(".zoneDragDrop").on("dragenter", onDragEnter);
    $(".zoneDragDrop").on("dragover", onDragOver);
    $(".zoneDragDrop").on("dragleave", onDragLeave);
    $(".zoneDragDrop").on("drop", onDrop);
}


function onDrop()
{
    $(".zoneDragDrop").removeClass("drop-active").addClass("fd-zone");
}


function onDragEnter(ev) {
    $(".zoneDragDrop").removeClass("fd-zone").addClass("drop-active");

}


function onDragLeave(ev) {
    $(".zoneDragDrop").removeClass("drop-active").addClass("fd-zone");

}


function onDragOver(ev) {
    $(".zoneDragDrop").removeClass("fd-zone").addClass("drop-active");
}