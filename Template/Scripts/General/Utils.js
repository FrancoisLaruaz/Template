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
    catch (err) {
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

function ShowSpinner() {
    $('#spinner').fadeIn();
}

function HideSpinner() {
    $('#spinner').fadeOut();
}

function ErrorActions() {
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

            if ($("#loginOrSignInModal").is(":visible") )
            {
  
                if ($("#SubmitButtonSignUp").is(":visible"))
                {
                    IdElement = 'SubmitButtonSignUp';
                }
                else if ($("#SubmitButtonLogin").is(":visible"))
                {
                    IdElement = 'SubmitButtonLogin';
                }
                else {
                    IdElement = null;
                    e.preventDefault();
                }

            }
           

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


function RemoveDiv(IdElement) {
    var divIdToRemove = "#" + IdElement;
    $(divIdToRemove).fadeOut(500, function () {
        $(this).remove();
    })
}

function OpenNewTabWindow(url) {
    if (url != null && url.trim() != '') {
        var win = window.open(url, '_blank');
        win.focus();
    }
}


function ScrollToErrorOrFirstInput(formname) {
    var FieldToFocus = $('#' + formname + ' .input-validation-error:first').get(0);
    if (FieldToFocus == null) {
        FieldToFocus = $('#' + formname + ' :input:not(input[type=button],input[type=submit],button):visible:first').get(0);
    }

    if (FieldToFocus != null) {
        $('html, body').animate({
            scrollTop: $(FieldToFocus).offset().top - 100
        }, 1000);

        $(FieldToFocus).focus();
    }
    else {
        BackToTop();
    }

}


jQuery.exists = function (selector) { return ($(selector).length > 0); }

function SetValidationForm(IdForm) {
    var Form = $("#" + IdForm);
    if ($(Form).length > 0) {
        $(Form).removeData("validator");
        $(Form).removeData("unobtrusiveValidation");
        $.validator.unobtrusive.parse("#" + IdForm);
    }
}

function ContainSpecialCharacter(str) {
    return /[~`!#$%\^&*+=\-\[\]\\';,/{}|\\":<>\?]/g.test(str);
}

function PopupGenericWindow(url, title, w, h) {
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


function SetDateTimeFields() {
    $('.DateTimeField').each(function (index, value) {
        var Id = $(this).attr('id');
        $('#' + Id).datetimepicker({
            format: 'MM/DD/YYYY hh:mm:ss',
            defaultDate: new Date()
        });

    });
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

        } else {
            ScrollToErrorOrFirstInput(FormId);
        }
    });

}


function scrollToErrorOnly(formname) {
    var FieldToFocus = $('#' + formname + ' .input-validation-error:first').get(0);
    if (FieldToFocus != null) {
        $('html, body').animate({
            scrollTop: $(FieldToFocus).offset().top - 120
        }, 1000);

        $(FieldToFocus).focus();
    }
}


function CorrectCharacters(string) {
 
    var retour = string;
    if (retour != null) {
        retour = retour.replace('&#000;', '^@ '.trim());
        retour = retour.replace('&#001;', '^A '.trim());
        retour = retour.replace('&#002;', '^B '.trim());
        retour = retour.replace('&#003;', '^C '.trim());
        retour = retour.replace('&#004;', '^D '.trim());
        retour = retour.replace('&#005;', '^E '.trim());
        retour = retour.replace('&#006;', '^F '.trim());
        retour = retour.replace('&#007;', '^G '.trim());
        retour = retour.replace('&#008;', '^H '.trim());
        retour = retour.replace('&#009;', '^I '.trim());
        retour = retour.replace('&#010;', '^J '.trim());
        retour = retour.replace('&#011;', '^K '.trim());
        retour = retour.replace('&#012;', '^L '.trim());
        retour = retour.replace('&#013;', '^M '.trim());
        retour = retour.replace('&#014;', '^N '.trim());
        retour = retour.replace('&#015;', '^O '.trim());
        retour = retour.replace('&#016;', '^P '.trim());
        retour = retour.replace('&#017;', '^Q '.trim());
        retour = retour.replace('&#018;', '^R '.trim());
        retour = retour.replace('&#019;', '^S '.trim());
        retour = retour.replace('&#020;', '^T '.trim());
        retour = retour.replace('&#021;', '^U '.trim());
        retour = retour.replace('&#022;', '^V '.trim());
        retour = retour.replace('&#023;', '^W '.trim());
        retour = retour.replace('&#024;', '^X '.trim());
        retour = retour.replace('&#025;', '^Y '.trim());
        retour = retour.replace('&#026;', '^Z '.trim());
        retour = retour.replace('&#027;', '^[ '.trim());
        retour = retour.replace('&#028;', '^\ '.trim());
        retour = retour.replace('&#029;', '^] '.trim());
        retour = retour.replace('&#030;', '^^ '.trim());
        retour = retour.replace('&#031;', '^_ '.trim());
        retour = retour.replace('&#032;', '  	'.trim());
        retour = retour.replace('&#033;', '! 	'.trim());
        retour = retour.replace('&#034;', '" 	'.trim());
        retour = retour.replace('&#035;', '# 	'.trim());
        retour = retour.replace('&#036;', '$ 	'.trim());
        retour = retour.replace('&#037;', '% 	'.trim());
        retour = retour.replace('&#038;', '& 	'.trim());
        retour = retour.replace('&#039;', "'".trim());
        retour = retour.replace('&#040;', '( 	'.trim());
        retour = retour.replace('&#041;', ') 	'.trim());
        retour = retour.replace('&#042;', '* 	'.trim());
        retour = retour.replace('&#043;', '+ 	'.trim());
        retour = retour.replace('&#044;', ', 	'.trim());
        retour = retour.replace('&#045;', '- 	'.trim());
        retour = retour.replace('&#046;', '. 	'.trim());
        retour = retour.replace('&#047;', '/ 	'.trim());
        retour = retour.replace('&#048;', '0 	'.trim());
        retour = retour.replace('&#049;', '1 	'.trim());
        retour = retour.replace('&#050;', '2 	'.trim());
        retour = retour.replace('&#051;', '3 	'.trim());
        retour = retour.replace('&#052;', '4 	'.trim());
        retour = retour.replace('&#053;', '5 	'.trim());
        retour = retour.replace('&#054;', '6 	'.trim());
        retour = retour.replace('&#055;', '7 	'.trim());
        retour = retour.replace('&#056;', '8 	'.trim());
        retour = retour.replace('&#057;', '9 	'.trim());
        retour = retour.replace('&#058;', ': 	'.trim());
        retour = retour.replace('&#059;', '; 	'.trim());
        retour = retour.replace('&#060;', '< 	'.trim());
        retour = retour.replace('&#061;', '= 	'.trim());
        retour = retour.replace('&#062;', '> 	'.trim());
        retour = retour.replace('&#063;', '? 	'.trim());
        retour = retour.replace('&#064;', '@ 	'.trim());
        retour = retour.replace('&#065;', 'A 	'.trim());
        retour = retour.replace('&#066;', 'B 	'.trim());
        retour = retour.replace('&#067;', 'C 	'.trim());
        retour = retour.replace('&#068;', 'D 	'.trim());
        retour = retour.replace('&#069;', 'E 	'.trim());
        retour = retour.replace('&#070;', 'F 	'.trim());
        retour = retour.replace('&#071;', 'G 	'.trim());
        retour = retour.replace('&#072;', 'H 	'.trim());
        retour = retour.replace('&#073;', 'I 	'.trim());
        retour = retour.replace('&#074;', 'J 	'.trim());
        retour = retour.replace('&#075;', 'K 	'.trim());
        retour = retour.replace('&#076;', 'L 	'.trim());
        retour = retour.replace('&#077;', 'M 	'.trim());
        retour = retour.replace('&#078;', 'N 	'.trim());
        retour = retour.replace('&#079;', 'O 	'.trim());
        retour = retour.replace('&#080;', 'P 	'.trim());
        retour = retour.replace('&#081;', 'Q 	'.trim());
        retour = retour.replace('&#082;', 'R 	'.trim());
        retour = retour.replace('&#083;', 'S 	'.trim());
        retour = retour.replace('&#084;', 'T 	'.trim());
        retour = retour.replace('&#085;', 'U 	'.trim());
        retour = retour.replace('&#086;', 'V 	'.trim());
        retour = retour.replace('&#087;', 'W 	'.trim());
        retour = retour.replace('&#088;', 'X 	'.trim());
        retour = retour.replace('&#089;', 'Y 	'.trim());
        retour = retour.replace('&#090;', 'Z 	'.trim());
        retour = retour.replace('&#091;', '[ 	'.trim());
        retour = retour.replace('&#092;', '\ 	'.trim());
        retour = retour.replace('&#093;', '] 	'.trim());
        retour = retour.replace('&#094;', '^ 	'.trim());
        retour = retour.replace('&#095;', '_ 	'.trim());
        retour = retour.replace('&#096;', '` 	'.trim());
        retour = retour.replace('&#097;', 'a 	'.trim());
        retour = retour.replace('&#098;', 'b 	'.trim());
        retour = retour.replace('&#099;', 'c 	'.trim());
        retour = retour.replace('&#100;', 'd 	'.trim());
        retour = retour.replace('&#101;', 'e 	'.trim());
        retour = retour.replace('&#102;', 'f 	'.trim());
        retour = retour.replace('&#103;', 'g 	'.trim());
        retour = retour.replace('&#104;', 'h 	'.trim());
        retour = retour.replace('&#105;', 'i 	'.trim());
        retour = retour.replace('&#106;', 'j 	'.trim());
        retour = retour.replace('&#107;', 'k 	'.trim());
        retour = retour.replace('&#108;', 'l 	'.trim());
        retour = retour.replace('&#109;', 'm 	'.trim());
        retour = retour.replace('&#110;', 'n 	'.trim());
        retour = retour.replace('&#111;', 'o 	'.trim());
        retour = retour.replace('&#112;', 'p 	'.trim());
        retour = retour.replace('&#113;', 'q 	'.trim());
        retour = retour.replace('&#114;', 'r 	'.trim());
        retour = retour.replace('&#115;', 's 	'.trim());
        retour = retour.replace('&#116;', 't 	'.trim());
        retour = retour.replace('&#117;', 'u 	'.trim());
        retour = retour.replace('&#118;', 'v 	'.trim());
        retour = retour.replace('&#119;', 'w 	'.trim());
        retour = retour.replace('&#120;', 'x 	'.trim());
        retour = retour.replace('&#121;', 'y 	'.trim());
        retour = retour.replace('&#122;', 'z 	'.trim());
        retour = retour.replace('&#123;', '{ 	'.trim());
        retour = retour.replace('&#124;', '| 	'.trim());
        retour = retour.replace('&#125;', '} 	'.trim());
        retour = retour.replace('&#126;', '~ 	'.trim());
        retour = retour.replace('&#127;', '^? '.trim());
        retour = retour.replace('&#128;', '  	'.trim());
        retour = retour.replace('&#129;', '  	'.trim());
        retour = retour.replace('&#130;', '  	'.trim());
        retour = retour.replace('&#131;', '  	'.trim());
        retour = retour.replace('&#132;', '  	'.trim());
        retour = retour.replace('&#133;', '  	'.trim());
        retour = retour.replace('&#134;', '  	'.trim());
        retour = retour.replace('&#135;', '  	'.trim());
        retour = retour.replace('&#136;', '  	'.trim());
        retour = retour.replace('&#137;', '  	'.trim());
        retour = retour.replace('&#138;', '  	'.trim());
        retour = retour.replace('&#139;', '  	'.trim());
        retour = retour.replace('&#140;', '  	'.trim());
        retour = retour.replace('&#141;', '  	'.trim());
        retour = retour.replace('&#142;', '  	'.trim());
        retour = retour.replace('&#143;', '  	'.trim());
        retour = retour.replace('&#144;', '  	'.trim());
        retour = retour.replace('&#145;', '  	'.trim());
        retour = retour.replace('&#146;', '  	'.trim());
        retour = retour.replace('&#147;', '  	'.trim());
        retour = retour.replace('&#148;', '  	'.trim());
        retour = retour.replace('&#149;', '  	'.trim());
        retour = retour.replace('&#150;', '  	'.trim());
        retour = retour.replace('&#151;', '  	'.trim());
        retour = retour.replace('&#152;', '  	'.trim());
        retour = retour.replace('&#153;', '  	'.trim());
        retour = retour.replace('&#154;', '  	'.trim());
        retour = retour.replace('&#155;', '  	'.trim());
        retour = retour.replace('&#156;', '  	'.trim());
        retour = retour.replace('&#157;', '  	'.trim());
        retour = retour.replace('&#158;', '  	'.trim());
        retour = retour.replace('&#159;', '  	'.trim());
        retour = retour.replace('&#160;', '  	'.trim());
        retour = retour.replace('&#161;', '¡ 	'.trim());
        retour = retour.replace('&#162;', '¢ 	'.trim());
        retour = retour.replace('&#163;', '£ 	'.trim());
        retour = retour.replace('&#164;', '¤ 	'.trim());
        retour = retour.replace('&#165;', '¥ 	'.trim());
        retour = retour.replace('&#166;', '¦ 	'.trim());
        retour = retour.replace('&#167;', '§ 	'.trim());
        retour = retour.replace('&#168;', '¨ 	'.trim());
        retour = retour.replace('&#169;', '© 	'.trim());
        retour = retour.replace('&#170;', 'ª 	'.trim());
        retour = retour.replace('&#171;', '« 	'.trim());
        retour = retour.replace('&#172;', '¬ 	'.trim());
        retour = retour.replace('&#173;', '­ 	'.trim());
        retour = retour.replace('&#174;', '® 	'.trim());
        retour = retour.replace('&#175;', '¯ 	'.trim());
        retour = retour.replace('&#176;', '° 	'.trim());
        retour = retour.replace('&#177;', '± 	'.trim());
        retour = retour.replace('&#178;', '² 	'.trim());
        retour = retour.replace('&#179;', '³ 	'.trim());
        retour = retour.replace('&#180;', '´ 	'.trim());
        retour = retour.replace('&#181;', 'µ 	'.trim());
        retour = retour.replace('&#182;', '¶ 	'.trim());
        retour = retour.replace('&#183;', '· 	'.trim());
        retour = retour.replace('&#184;', '¸ 	'.trim());
        retour = retour.replace('&#185;', '¹ 	'.trim());
        retour = retour.replace('&#186;', 'º 	'.trim());
        retour = retour.replace('&#187;', '» 	'.trim());
        retour = retour.replace('&#188;', '¼ 	'.trim());
        retour = retour.replace('&#189;', '½ 	'.trim());
        retour = retour.replace('&#190;', '¾ 	'.trim());
        retour = retour.replace('&#191;', '¿ 	'.trim());
        retour = retour.replace('&#192;', 'À 	'.trim());
        retour = retour.replace('&#193;', 'Á 	'.trim());
        retour = retour.replace('&#194;', 'Â 	'.trim());
        retour = retour.replace('&#195;', 'Ã 	'.trim());
        retour = retour.replace('&#196;', 'Ä 	'.trim());
        retour = retour.replace('&#197;', 'Å 	'.trim());
        retour = retour.replace('&#198;', 'Æ 	'.trim());
        retour = retour.replace('&#199;', 'Ç 	'.trim());
        retour = retour.replace('&#200;', 'È 	'.trim());
        retour = retour.replace('&#201;', 'É 	'.trim());
        retour = retour.replace('&#202;', 'Ê 	'.trim());
        retour = retour.replace('&#203;', 'Ë 	'.trim());
        retour = retour.replace('&#204;', 'Ì 	'.trim());
        retour = retour.replace('&#205;', 'Í 	'.trim());
        retour = retour.replace('&#206;', 'Î 	'.trim());
        retour = retour.replace('&#207;', 'Ï 	'.trim());
        retour = retour.replace('&#208;', 'Ð 	'.trim());
        retour = retour.replace('&#209;', 'Ñ 	'.trim());
        retour = retour.replace('&#210;', 'Ò 	'.trim());
        retour = retour.replace('&#211;', 'Ó 	'.trim());
        retour = retour.replace('&#212;', 'Ô 	'.trim());
        retour = retour.replace('&#213;', 'Õ 	'.trim());
        retour = retour.replace('&#214;', 'Ö 	'.trim());
        retour = retour.replace('&#215;', '× 	'.trim());
        retour = retour.replace('&#216;', 'Ø 	'.trim());
        retour = retour.replace('&#217;', 'Ù 	'.trim());
        retour = retour.replace('&#218;', 'Ú 	'.trim());
        retour = retour.replace('&#219;', 'Û 	'.trim());
        retour = retour.replace('&#220;', 'Ü 	'.trim());
        retour = retour.replace('&#221;', 'Ý 	'.trim());
        retour = retour.replace('&#222;', 'Þ 	'.trim());
        retour = retour.replace('&#223;', 'ß 	'.trim());
        retour = retour.replace('&#224;', 'à 	'.trim());
        retour = retour.replace('&#225;', 'á 	'.trim());
        retour = retour.replace('&#226;', 'â 	'.trim());
        retour = retour.replace('&#227;', 'ã 	'.trim());
        retour = retour.replace('&#228;', 'ä 	'.trim());
        retour = retour.replace('&#229;', 'å 	'.trim());
        retour = retour.replace('&#230;', 'æ 	'.trim());
        retour = retour.replace('&#231;', 'ç 	'.trim());
        retour = retour.replace('&#232;', 'è 	'.trim());
        retour = retour.replace('&#233;', 'é 	'.trim());
        retour = retour.replace('&#234;', 'ê 	'.trim());
        retour = retour.replace('&#235;', 'ë 	'.trim());
        retour = retour.replace('&#236;', 'ì 	'.trim());
        retour = retour.replace('&#237;', 'í 	'.trim());
        retour = retour.replace('&#238;', 'î 	'.trim());
        retour = retour.replace('&#239;', 'ï 	'.trim());
        retour = retour.replace('&#240;', 'ð 	'.trim());
        retour = retour.replace('&#241;', 'ñ 	'.trim());
        retour = retour.replace('&#242;', 'ò 	'.trim());
        retour = retour.replace('&#243;', 'ó 	'.trim());
        retour = retour.replace('&#244;', 'ô 	'.trim());
        retour = retour.replace('&#245;', 'õ 	'.trim());
        retour = retour.replace('&#246;', 'ö 	'.trim());
        retour = retour.replace('&#247;', '÷ 	'.trim());
        retour = retour.replace('&#248;', 'ø 	'.trim());
        retour = retour.replace('&#249;', 'ù 	'.trim());
        retour = retour.replace('&#250;', 'ú 	'.trim());
        retour = retour.replace('&#251;', 'û 	'.trim());
        retour = retour.replace('&#252;', 'ü 	'.trim());
        retour = retour.replace('&#253;', 'ý 	'.trim());
        retour = retour.replace('&#254;', 'þ 	'.trim());
        retour = retour.replace('&#255;', 'ÿ 	'.trim());
    }

    return retour;
}


function replaceAll(str, term, replacement) {
    return str.replace(new RegExp(escapeRegExp(term), 'g'), replacement);
}