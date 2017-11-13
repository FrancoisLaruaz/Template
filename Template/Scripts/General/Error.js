
window.onerror = function (_errorMsg, _url, _lineNumber, _col, _error) {
    try {
        var nVer = navigator.appVersion;
        var nAgt = navigator.userAgent;
        var browserName = navigator.appName;
        var fullVersion = '' + parseFloat(navigator.appVersion);
        var majorVersion = parseInt(navigator.appVersion, 10);
        var nameOffset, verOffset, ix;

        // In Opera, the true version is after "Opera" or after "Version"
        if ((verOffset = nAgt.indexOf("Opera")) != -1) {
            browserName = "Opera";
            fullVersion = nAgt.substring(verOffset + 6);
            if ((verOffset = nAgt.indexOf("Version")) != -1)
                fullVersion = nAgt.substring(verOffset + 8);
        }
        // In MSIE, the true version is after "MSIE" in userAgent
        else if ((verOffset = nAgt.indexOf("MSIE")) != -1) {
            browserName = "Microsoft Internet Explorer";
            fullVersion = nAgt.substring(verOffset + 5);
        }
        // In Chrome, the true version is after "Chrome" 
        else if ((verOffset = nAgt.indexOf("Chrome")) != -1) {
            browserName = "Chrome";
            fullVersion = nAgt.substring(verOffset + 7);
        }
        // In Safari, the true version is after "Safari" or after "Version" 
        else if ((verOffset = nAgt.indexOf("Safari")) != -1) {
            browserName = "Safari";
            fullVersion = nAgt.substring(verOffset + 7);
            if ((verOffset = nAgt.indexOf("Version")) != -1)
                fullVersion = nAgt.substring(verOffset + 8);
        }
        // In Firefox, the true version is after "Firefox" 
        else if ((verOffset = nAgt.indexOf("Firefox")) != -1) {
            browserName = "Firefox";
            fullVersion = nAgt.substring(verOffset + 8);
        }
        // In most other browsers, "name/version" is at the end of userAgent 
        else if ((nameOffset = nAgt.lastIndexOf(' ') + 1) <
            (verOffset = nAgt.lastIndexOf('/'))) {
            browserName = nAgt.substring(nameOffset, verOffset);
            fullVersion = nAgt.substring(verOffset + 1);
            if (browserName.toLowerCase() == browserName.toUpperCase()) {
                browserName = navigator.appName;
            }
        }

        // trim the fullVersion string at semicolon/space if present
        if ((ix = fullVersion.indexOf(";")) != -1)
            fullVersion = fullVersion.substring(0, ix);
        if ((ix = fullVersion.indexOf(" ")) != -1)
            fullVersion = fullVersion.substring(0, ix);

        majorVersion = parseInt('' + fullVersion, 10);
        if (isNaN(majorVersion)) {
            fullVersion = '' + parseFloat(navigator.appVersion);
            majorVersion = parseInt(navigator.appVersion, 10);
        }
    }
    catch (err) {
        JSCaughtException(err);
    }

    try {
        var strError = "";
        if (browserName == null)
            browserName = "";
        if (fullVersion == null)
            fullVersion = "";
        var _browser = browserName + " " + fullVersion;
        if (typeof _error == "undefined" || _error == null) {
            strError = "";
        }
        else {
            strError = _error.toString();
        }
        $.ajax({
            url: '/Error/LogJavascriptError',
            type: 'POST',
            data: { errorMsg: _errorMsg, url: _url, lineNumber: _lineNumber, col: _col, error: strError, browser: _browser },
            success: function (data) {
                HideSpinner(); 
                if (IsTestEnvironment()) {

                    if (typeof NotificationKOErrorDev !== "undefined") {
                        NotificationKOErrorDev("[ERROR] You have just generated a js error : <br><br> " + _errorMsg + "<br><br> Please check the full <a style='text-decoration: underline;' onclick='OpenLogPage()'>LOG </a> ");
                    }
                    else {
                        alert("[ERROR] You have just generated a js error : <br><br> " + _errorMsg + "<br><br> Please check the full <a style='text-decoration: underline;' onclick='OpenLogPage()'>LOG </a> ");
                    }
                }
            },
            error: function (xhr, status, error) {
                HideSpinner(); 
            }
        });
    }
    catch (err) {
        HideSpinner(); 
        JSCaughtException(err);
    }
};

function IsTestEnvironment()
{
    var result = false;
    if (location.hostname == '127.0.0.1' || location.hostname == 'localhost') {
        result = true;
    }
    return result;
}

function JSCaughtException(err)
{
    
    if (IsTestEnvironment())
    {
        var message = "";
        if (err != null)
        {
            message = err.toString();
        }

        NotificationKO(message);
    }
}

function OpenLogPage()
{
    var baseUrl = GetHomePageUrl();

    var url = baseUrl + '/Admin/Logs';
    var win = window.open(url, '_blank');
    win.focus();
}

