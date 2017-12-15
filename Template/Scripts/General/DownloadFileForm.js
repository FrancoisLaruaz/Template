function DownloadEncryptedFile(idElementSrc) {

    var Path = $("#" + idElementSrc).val();
    if (Path != null && Path.trim() != '') {
        $('#form_download_file').attr('action', '../../Upload/DownloadEncryptedFile?PathFile=' + Path);
        $('#form_download_file').submit();
        $('#form_download_file').attr('action', '');
    }
}

