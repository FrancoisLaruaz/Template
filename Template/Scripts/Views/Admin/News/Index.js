$(document).ready(function () {
  //  RefreshData(true);
    SetEnterKey("SearchIcon");


    //BEGIN get edit update form
    $(document).on("click", ".element-card", function () {
        ShowSpinner();
        var elementId = $(this).data("news_id");
        window.location.href = GetHomePageUrl() + '/Admin/EditNews/' + elementId;

    })
    //END get edit update form

    $(document).on("click", ".delnewsBtn", function (e) {
        e.stopPropagation();
        var elementId = $(this).data("news_id");
        console.log("elementId to del->", elementId);
        SweetConfirmation("[[[Are you sure about deleting this news ?]]]", null, function () {
            $("#spinner").fadeIn();
            //BEGIN ajax delete
            $.ajax({
                url: "/Admin/DeleteNews",
                type: "POST",
                data: { id: elementId },
                success: function (data) {
                    if (data.Success) {
                        NotificationOK("[[[The news has been successfully deleted.]]]");
                        var divIdToRemove = "#elementWrap_" + data.Id;
                        $(divIdToRemove).fadeOut(function () {
                            $(this).remove();
                        });
                    } else {
                        NotificationKO(data.Err);
                       
                    }
                },
                error: function (xhr, error) {
                    console.log(" : error" + error);
                    NotificationKO("[[[Error occured, please try again later.]]]");
                }
            });
            //END ajax delete
        });

    });
    //END delete update

});




function RefreshData(IsNewResearch) {

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
        url: "/Admin/_DisplayPublishedNews",
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