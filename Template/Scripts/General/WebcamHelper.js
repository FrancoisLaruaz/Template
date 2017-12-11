function SetCamera(Purpose, IdFileToSave) {

    var WebcamSessionId = GetRandomInt(0, 99999999);

    $(".webcamBtn").unbind("click");
    $(".webcamBtn").on("click", function (e) {
        e.preventDefault();
        ShowSpinner();
        WebcamCapture();
    });

    var UrlSave = "/Upload/SaveWebcamCapture?Purpose=" + Purpose + "&WebcamSessionId=" + WebcamSessionId.toString();
    var UrlGet = "/Upload/GetWebcamCapture?Purpose=" + Purpose + "&WebcamSessionId=" + WebcamSessionId.toString();

    $("#XwebcamXobjectX").css("width", "100%");
    $("#Camera").webcam({
        mode: "save",
        quality: 90,
        swffile: "/Scripts/OtherFormats/jscam.swf",
        onTick: function (remain) {
        },
        onSave: function (data) {
            ShowSpinner();
            $.ajax({
                type: "POST",
                url: UrlGet,
                data: '',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data != null) {
                        if (data.Result) {
                            if ($("#CameraPreview").length > 0 && data.PathFilePreview != null) {

                                if ($("#CameraPreview").is("img")) {
                                    $("#CameraPreview").attr("src", data.PathFilePreview);
                                }
                                else {
                                    $('#CameraPreview').css('backgroundImage', 'url(' + data.PathFilePreview + ')');
                                }
                                $("#CameraPreview").fadeIn();
                            }
                            if (IdFileToSave != null && $("#" + IdFileToSave).length > 0 && data.PathFile != null) {
                                $("#" + IdFileToSave).val(data.PathFile);
                            }
                        }
                    }
                    HideSpinner();
                },
                failure: function (response) {
                    ErrorActions();
                }
            });
        },
        noCameraFound: function () {
            NotificationKO('[[[Web camera is not available]]]');
            HideSpinner();
        },
        error: function (e) {
            NotificationKO('[[[Internal camera plugin error]]]');
            HideSpinner();
        },
        onCapture: function () {
            ShowSpinner();
            webcam.save(UrlSave);
        },
        debug: function () {
        },
        onLoad: function () {
            if (!CanUserTakeWebcamPicture()) {
                $("#Camera").hide();
                $("#CameraNotFound").fadeIn();
                $(".webcamBtn").hide();
            }
            else {
                $("#CameraNotFound").hide();
                $("#Camera").fadeIn();
                $(".webcamBtn").show();
            }
        }
    });
    $("#XwebcamXobjectX").css("width", "100%");


}




function WebcamCapture() {
    PlayAudio(WebCamPictureAudio);
    ShowSpinner();
    webcam.capture();
    return false;
}