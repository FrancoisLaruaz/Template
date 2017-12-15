﻿var DefaultMaxSizeFile = 10000000;
var PdfExtension = ".pdf";
var WordExtension = ".doc,.docx";

function SetDragAndDropEvents(idElementZone) {

    var selector = $(".zoneDragDrop")
    if (idElementZone == null || typeof idElementZone == undefined) {
        idElementZone = null;
    }
    else {
        selector = $("#" + idElementZone);
    }


    $(selector).on("dragenter", function () {
        onDragEnter(idElementZone);
    });
    $(selector).on("dragover", function () {
        onDragOver(idElementZone);
    });
    $(selector).on("dragleave", function () {
        onDragLeave(idElementZone);
    });
    $(selector).on("drop", function () {
        onDrop(idElementZone);
    });
}


function onDrop(idElementZone) {
    var selector = $(".zoneDragDrop");

    if (idElementZone != null) {
        selector = $("#" + idElementZone);
    }

    $(selector).removeClass("drop-active").addClass("fd-zone");
}


function onDragEnter(idElementZone) {
    var selector = $(".zoneDragDrop");
    if (idElementZone != null) {
        selector = $("#" + idElementZone);
    }
    $(selector).removeClass("fd-zone").addClass("drop-active");

}


function onDragLeave(idElementZone) {
    var selector = $(".zoneDragDrop");
    if (idElementZone != null) {
        selector = $("#" + idElementZone);

    }
    $(selector).removeClass("drop-active").addClass("fd-zone");

}


function onDragOver(idElementZone) {
    var selector = $(".zoneDragDrop");
    if (idElementZone != null) {
        selector = $("#" + idElementZone);
    }
    $(selector).removeClass("fd-zone").addClass("drop-active");
}


function SetDragAndDropPicture(idElementZone, Purpose, idImage) {

    SetDragAndDropEvents(idElementZone);

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
            model.append(file.nativeFile.name, file.nativeFile);
            var url = URL.createObjectURL(file.nativeFile);

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

                            if ($('#' + idImage + 'Preview').length > 0 && url != null && url != "") {
                                $('#' + idImage + 'Preview').attr('src', url);
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


function SetDragAndDropDocument(idElementZone, Purpose, OnUploadSuccessFunction, MaxSize, AllowedExtensions) {

    SetDragAndDropEvents(idElementZone);

    if (MaxSize == 0 || MaxSize == null) {
        MaxSize = DefaultMaxSizeFile;
    }

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
            model.append("AllowedExtensions", AllowedExtensions);
            model.append("MaxSize", MaxSize);
            model.append(file.nativeFile.name, file.nativeFile);
            var url = URL.createObjectURL(file.nativeFile);

            $.ajax({
                url: "/Upload/UploadDocument",
                type: 'POST',
                dataType: 'json',
                data: model,
                processData: false,
                contentType: false,// not json
                success: function (data) {
                    if (data.Result) {

                        if (data.PathFile != null && data.PathFile != "") {
                            if (typeof (OnUploadSuccessFunction) === "function") {
                                OnUploadSuccessFunction.apply(this, [data.PathFile]);
                            }
                            NotificationOK("[[[The document has been successfully uploaded.]]]");
                        }
                    }
                    else if (data.Error != null && data.Error.trim() != "") {
                        NotificationKO(data.Error);
                    }
                    else {
                        NotificationKO("[[[Error while uploading the document.]]]");
                    }
                    HideSpinner();
                },
                error: function (response) {
                    NotificationKO("[[[Error while uploading the document.]]]");
                    HideSpinner();
                }
            });
        })
    })
}