var KeyPressAllowed = true;

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
    KeyPressAllowed = true;

    if (IdElement === undefined) {
        IdElement = null;
    }
    $(document).keydown(function (e) {
        if (e.repeat != undefined) {
            KeyPressAllowed = !e.repeat;
        }
        if (!KeyPressAllowed)
            return;
        KeyPressAllowed = false;

        if (e.which == 13) {
            if (IdElement != null) {
                var Element = $("#" + IdElement);
            
                if ($("#" + IdElement).length > 0 && $(Element).css("display") != "none" && !$(Element).is(':disabled') && !$(Element).is('.disabled') && $(Element).is(":visible") && !$(e.target).is("textarea")) {
                    e.preventDefault();
                    $(Element).click();
                    return false;
                }
            }
        }
    });


    $(document).keyup(function (e) {
        KeyPressAllowed = true;
    });
    $(document).focus(function (e) {
        KeyPressAllowed = true;
    });
}


function ScrollToErrorOrFirstInput(formname) {
    var FieldToFocus = $('#'+formname + ' .input-validation-error:first').get(0);
    if (FieldToFocus == null) {
        var FieldToFocus = $('#' +formname + ' :input:not(input[type=button],input[type=submit],button):visible:first').get(0);
    }

    if (FieldToFocus != null) {
        $('html, body').animate({
            scrollTop: $(FieldToFocus).offset().top -100
        }, 1000);

        $(FieldToFocus).focus();
    }
    else {
        BackToTop();
    }

}


jQuery.exists = function(selector) {return ($(selector).length > 0);}

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


function ScrollAndFocusToElement(id) {

    if ($("#" + id).length > 0) {
        $('html, body').animate({
            scrollTop: $("#" + id).offset().top - 120
        }, 1000);
        $("#" + id).focus();
    }
}


function GetRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}




function SetGenericAjaxForm(FormId, OnSuccess, Onfailure, OnBegin) {
    $("#" + FormId).unbind("submit");
    $("#" + FormId).on("submit", function (e) {
        e.preventDefault();
        var Form = $(this);
        var Model = $(Form).serialize();
        var url = $(Form).attr('action');
        if ($(this).valid()) {

            if (typeof (OnBegin) === "function") {
                OnBegin.apply(this, null);
            }

            $.ajax({
                url: url,
                type: "POST",
                data: Model,
                success: function (data) {

                    if (typeof (OnSuccess) === "function") {
                        OnSuccess.apply(this, [data]);
                    }
                    else {
                        HideSpinner();
                    }
                },
                error: function (xhr, error) {
                    if (typeof (Onfailure) === "function") {
                        Onfailure.apply(this, null);
                    }
                    else {
                        ErrorActions();
                    }
                }
            });
        }
    });
}