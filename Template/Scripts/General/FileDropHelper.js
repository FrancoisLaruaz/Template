

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

function SetDragAndDropPicture(idElementZone, Purpose, idImage) {

    SetDragAndDropEvents();

    // We can deal with iframe uploads using this URL:
    var options = {}
    // 'zone' is an ID but you can also give a DOM node:
    var zone = new FileDrop(idElementZone, options)

    // Do something when a user chooses or drops a file:
    zone.event('send', function (files) {
        // FileList might contain multiple items.
        files.each(function (file) {
            ShowSpinner();
            var model = new FormData();
            model.append("Purpose", Purpose);
            model.append("EncryptFile", true);
            model.append(file.nativeFile.name, file.nativeFile);

            $.ajax({
                url: "/Upload/UploadPicture",
                type: 'POST',
                dataType: 'json',
                data: model,
                processData: false,
                contentType: false,// not json
                success: function (data) {
                    if (data.Result) {

                        if (data.PathFile != null && data.PathFile != "") {
                            
                            if ($('#' + idImage + 'Preview').length > 0 && data.PathFilePreview != null && data.PathFilePreview != "") {
                                $('#' + idImage + 'Preview').attr('src', data.PathFilePreview);
                            }
                            $('#' + idImage).val(data.PathFile);
                        }
                    }
                    else if (data.Error != null && data.Error.trim() != "") {
                        NotificationKO(data.Error);
                    }
                    else {
                        NotificationKO("[[[Error while uploading the picture.]]]");
                    }
                    HideSpinner();
                },
                error: function (response) {
                    NotificationKO("[[[Error while uploading the picture.]]]");
                    HideSpinner();
                }
            });
        })
    })
}