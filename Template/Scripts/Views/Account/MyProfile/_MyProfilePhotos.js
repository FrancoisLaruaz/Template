$(document).ready(function () {
    SetInputFile();
    HideSpinner();
});


function SetInputFile()
{
    $('#HiddenPic').change(function (event) {

        if (event.target != null && event.target.files.length > 0) {

            var file = event.target.files[0];
            var IsSizeOk = file.size / 1024 / 1024 <= 0.5 ? true : false;
            if (IsSizeOk) {
                var model = new FormData();

                model.append("UserId", $("#HiddenUserId").val());
                model.append("Picture", file);


                $.ajax({
                    url: "/Account/UpdateMyProfilePicture",
                    type: 'POST',
                    dataType: 'json',
                    data: model,
                    processData: false,
                    contentType: false,// not json
                    success: function (data) {
                        if (data.Result) {
                            var url = URL.createObjectURL(file);
                            $('#PictureDiv').css('background-image', 'url(' + url + ')');
                            RefreshHeader();
                        }
                        else if (data.Error != null && data.Error.trim() != "") {
                            NotificationKO(data.Error);
                        }
                        else {
                            NotificationKO("[[[Error while uploading the file.]]]");
                        }
                    },
                    error: function (response) {
                        NotificationKO("[[[Error while uploading the file.]]]");
                    }
                });

            }
            else {
                NotificationKO('[[[Your file size exceeds 500 KB. Please upload another picture.]]]');
                $('#HiddenPic').val('');
            }
        }
    });
}

