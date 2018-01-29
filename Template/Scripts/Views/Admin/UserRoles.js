$(document).ready(function () {
    $('#SearchBox').focus();
    RefreshData(true);
    SetEnterKey("SearchIcon");
});

function RefreshData(IsNewResearch)
{

    if (IsNewResearch === undefined) {
        IsNewResearch = true;
    }

    if (IsNewResearch)
    {
        $('#StartAt').val(0);
    }
    StartAt = parseInt($('#StartAt').val());
    ShowSpinner();
    var _Pattern = $('#SearchBox').val().replace("'","''");
    $.ajax({
        url: "/Admin/_DisplayUserRoles",
        type: "POST",
        data: { Pattern: _Pattern, StartAt: StartAt },
        success: function (data) {
            BackToTop();
            if (data == "ERROR") {
                ErrorActions();
            }
            else {
                $("#targetContainer").fadeOut(500,function () {
                    $("#targetContainer").html(data).fadeIn(500);
                });
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}





function AddUserRole(roleid, userid) {
    ShowSpinner();
    $.ajax({
        url: "/Admin/AddUserRole",
        type: "POST",
        data: { roleid: roleid, userid: userid },
        success: function (data) {
            if (data != null && data.Result) {
                RefreshUserRoles(userid)
                NotificationOK("[[[The role has been successfully added to the user.]]]");
            }
            else if (data != null && data.Error != null) {
                NotificationKO(data.Error);
                HideSpinner();
            }
            else {
                ErrorActions();
            }
        },
        error: function (xhr, error) {
            ErrorActions();
        }
    });
}


function DeleteUserRole(roleid, userid) {

        ShowSpinner();
        $.ajax({
            url: "/Admin/DeleteUserRole",
            type: "POST",
            data: { roleid: roleid, userid: userid },
            success: function (data) {
                if (data != null && data.Result) {
                    RefreshUserRoles(userid)
                    NotificationOK("[[[The role has been successfully deleted.]]]");
                }
                else if (data != null && data.Error != null) {
                    NotificationKO(data.Error);
                    HideSpinner();
                }
                else {
                    ErrorActions();
                }
            },
            error: function (xhr, error) {
                ErrorActions();
            }
        });

}

function RefreshUserRoles(UserIdentityId) {
    if (UserIdentityId != null && UserIdentityId != '') {

        $.ajax({
            url: "/Admin/_DisplayUserRolesModifications",
            type: "POST",
            data: { UserIdentityId: UserIdentityId },
            success: function (data) {
                if (data == "ERROR") {
                    ErrorActions();
                }
                else {
                    $("#div_userroles_" + UserIdentityId).fadeOut(500, function () {
                        $("#div_userroles_" + UserIdentityId).html(data).fadeIn(500);
                    });
                    HideSpinner();
                }
            },
            error: function (xhr, error) {
                ErrorActions();
            }
        });
    }
    else {
        HideSpinner();
    }
}