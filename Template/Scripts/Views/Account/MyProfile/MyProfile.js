var actionActive = '';

$(document).ready(function () {

    SetNavActions();
    HideSpinner();
});


function SetNavActions()
{

    $(".navVerticalItem").unbind("click");
    $(".navVerticalItem").on("click", function (e) {
        var SelectedNav = $(this);

        var action = $(this).data("action");
        var _UserId = $("#HiddenUserId").val();
   
        if (actionActive != action && _UserId>0)
        {
            
            $.ajax({
                url: "/Account/" + action,
                type: "GET",
                data: { UserId: _UserId },
                success: function (data) {
                    if (data == "ERROR" || data==null) {
                        NotificationKO("[[[Unknown retrieval error]]]");
                    }
                    else if (data == "NotLoggedIn") {
                        NotificationKO("[[[Please log in to access your profile.]]]");
                    }
                    else {
                        $(".navVerticalItem").removeClass('navActive');
                        $(SelectedNav).addClass('navActive');
                        actionActive = action;
                        $("#divMyProfileAction").fadeOut(500,function () {
                            $("#divMyProfileAction").html(data).fadeIn(500);
                        });
                    }
                },
                error: function (xhr, error) {
                    ErrorActions();
                }
            });

        }

    });

    $("#EditProfilLink").click();

}