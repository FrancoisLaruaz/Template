function GetParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    var result = results[2];
    return result;
}

function BackToTop() {
    $('body,html').animate({
        scrollTop: 0
    }, 1000);
}

function GetHomePageUrl() {
    var result = null;
    try {
        var OldURL = window.location.href.toLowerCase();
        var newURL = OldURL;
        var prefix = "";
        if (OldURL.includes("https")) {
            prefix = "https://";
        }
        else if (OldURL.includes("http")) {
            prefix = "http://";
        }
        var tabURL = window.location.href.replace(prefix, "").split('/');

        if (tabURL.length > 0) {
            var nomSite = tabURL[0];
            newURL = prefix + nomSite;
        }

        if (newURL.split('?').length > 0) {
            newURL = newURL.split('?')[0];
        }
        result = newURL;
    }
    catch (err)
    {
        result = null;
    }
    return result;
}

function GoBackToHomePage() {
    var newUrl = GetHomePageUrl();
    if (newUrl != null) {
        window.location.href = GetHomePageUrl();
    }
}

function ShowSpinner(){
    $('#spinner').fadeIn();
}

function HideSpinner() {
    $('#spinner').fadeOut();
}

function ErrorActions()
{
    NotificationKO();
    HideSpinner();
}

function SetEnterKey(IdElement) {
    if (IdElement === undefined) {
        IdElement = null;
    }

    $(document).keypress(function (e) {
        if (e.which == 13) {
            if (IdElement != null) {
                var Element = $("#" + IdElement);
                if (Element && $(Element).css("display") != "none" && !$(Element).is(':disabled')) {
                    $(Element).click();
                }
            }
        }
    });
}