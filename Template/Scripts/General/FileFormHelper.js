function DownloadFileDirectPath(Path) {
    if (Path != null && Path.trim() != '') {
        $('#form_download_file').attr('action', '../../Upload/DownloadFile?PathFile=' + Path);
        $('#form_download_file').submit();
        $('#form_download_file').attr('action', '');
    }
}

function DownloadFileInElement(idElementSrc) {

    var Path = $("#" + idElementSrc).val();
    if (Path != null && Path.trim() != '') {
        $('#form_download_file').attr('action', '../../Upload/DownloadFile?PathFile=' + Path);
        $('#form_download_file').submit();
        $('#form_download_file').attr('action', '');
    }
}


function DeleteFileDirectPath(Path) {
    if (Path != null && Path.trim() != '') {
        $('#form_delete_file').attr('action', '../../Upload/DeleteFile?PathFile=' + Path);
        SetGenericAjaxForm('form_delete_file', null, null, null);
        $('#form_delete_file').submit();
        $('#form_delete_file').attr('action', '');
    }
}