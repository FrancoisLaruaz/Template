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
                if ($("#" + IdElement).length > 0 && $(Element).css("display") != "none" && !$(Element).is(':disabled') && $(Element).is(":visible")) {
                    $(Element).click();
                    e.preventDefault();
                    return false;
                }
            }
        }
    });
}


function SetValidationForm(IdForm) {
    var Form = $("#" + IdForm);
    if ($(Form).length>0) {
        $(Form).removeData("validator");
        $(Form).removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse("#" + IdForm);
    }
}

function ContainSpecialCharacter(str) {
    return /[~`!#$%\^&*+=\-\[\]\\';,/{}|\\":<>\?]/g.test(str);
}

function PopupWindow(url, title, w, h) {
    // ex :  onclick="popupwindow('https://www.linkedin.com/shareArticle?mini=true&url=@HttpUtility.UrlEncode(ViewBag.OgUrl)&title=@HttpUtility.UrlEncode(ViewBag.OgTitle)&summary=@HttpUtility.UrlEncode(ViewBag.OgDescription)', '@ViewBag.OgTitle', 520, 570);">

    var y = window.outerHeight / 2 + window.screenY - (h / 2);
    var x = window.outerWidth / 2 + window.screenX - (w / 2);
    return window.open(url, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width=' + w + ', height=' + h + ', top=' + y + ', left=' + x);
}