$(function () {

    $(".PasswordToCheck").on("propertychange change keyup paste input keypress change keydown", function () {
        SetPasswordForm(this);
    });

    setTimeout(function () {
        $('.PasswordToCheck').each(function (index, value) {
            SetPasswordForm(this);
        });
    }, 1000);

    $('.PasswordToCheck').each(function (index, value) {
        SetPasswordForm(this);
    });


});






function SetPasswordForm(passwordElement) {


    var disabledSubmitButton = false;
    var form = $(passwordElement).closest("form");
    alert($(form).attr('id'));
    var passWordButton = $(form).find(".PasswordButton");
    $(passWordButton).show();

    if ($(passwordElement).length > 0 && $(form).length > 0) {
        var Password = $(passwordElement).val();
        var upperCase = new RegExp('[A-Z]');
        var lowerCase = new RegExp('[a-z]');
        var numbers = new RegExp('[0-9]');

        var PasswordStrengthScore = 0;

        if (Password == null || typeof Password == "undefined" || Password.trim().length < 8) {
            disabledSubmitButton = true;
            SetIconKO("EightCharacters");
        }
        else {
            SetIconOK("EightCharacters");
        }

        if (Password == null || typeof Password == "undefined" || !Password.match(lowerCase)) {
            disabledSubmitButton = true;
            SetIconKO("LowerCaseLetter");
        }
        else {
            SetIconOK("LowerCaseLetter");
        }

        if (Password == null || typeof Password == "undefined" || !Password.match(upperCase)) {
            disabledSubmitButton = true;
            SetIconKO("UpperCaseLetter");
        }
        else {
            SetIconOK("UpperCaseLetter");
        }

        if (Password == null || typeof Password == "undefined" || !Password.match(numbers)) {
            disabledSubmitButton = true;
            SetIconKO("OneNumber");
        }
        else {
            SetIconOK("OneNumber");
        }


        if (disabledSubmitButton) {
            PasswordStrengthScore = 5;
        }
        else if (Password.trim().length >= 12 || ContainSpecialCharacter(Password)) {
            PasswordStrengthScore = 13;
        }
        else {
            PasswordStrengthScore = 11;
        }


        SetPasswordStrengthScore(PasswordStrengthScore, form);
        $(passWordButton).toggleClass("disabled", disabledSubmitButton);
        if (!disabledSubmitButton)
            $(passWordButton).removeAttr('disabled');
    }

}

function SetPasswordStrengthScore(Score, form) {
    var color = "red";
    var text = "Very weak";
    if (Score >= 13) {
        var color = "green";
        var text = "Very strong";
    }
    else if (Score >= 11) {
        var color = "green";
        var text = "Strong";
    }
    else if (Score >= 9) {
        var color = "blue";
        var text = "Good";
    }
    else if (Score >= 6) {
        var color = "yellow";
        var text = "Intermediate";
    }
    else if (Score >= 3) {
        var color = "red";
        var text = "Weak";
    }

    $(form).find(".ScorePasswordStrength").html(text);
    $(form).find(".textPasswordStrength").removeClass();
    $(form).find(".textPasswordStrength").addClass(color);
}

function SetIconOK(Element) {
    var IconElement = $("#Icon" + Element);
    if (IconElement) {
        $(IconElement).removeClass("red").removeClass("glyphicon-remove").addClass("glyphicon-ok").addClass("green");
    }
    var TextElement = $("#text" + Element);
    if (TextElement) {
        $(TextElement).removeClass("red").addClass("green");
    }
}

function SetIconKO(Element) {
    var IconElement = $("#Icon" + Element);
    if (IconElement) {
        $(IconElement).removeClass("glyphicon-ok").removeClass("green").addClass("red").addClass("glyphicon-remove");
    }
    var TextElement = $("#text" + Element);
    if (TextElement) {
        $(TextElement).removeClass("green").addClass("red");
    }
}