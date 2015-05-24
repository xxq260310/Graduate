(function (jq, otbsWindow) {
    var $ = jq;

    // disable the cache for jquery's ajax.
    $.ajaxSetup({ cache: false });

    //var getBvFeedLogInfo = function ()
    //{
    //    $("#bv_preview").click(function (e) {
    //        e.preventDefault;
    //        window.location.href = "/BvFeedLogs/Preview";
    //    })
    //}
    //var getBvFeedLogInfo = function () {
    //    $("#bv_preview").unbind("click").click(function (e) {
    //        e.preventDefault;
    //        var trList = $("#bv_feed_log_tbody").children("tr");
    //        for (var i = 0; i < trList.length; i++) {
    //            var tdList = trList.eq(i).find("td");
    //            var checked = tdList[0].children[0];

    //            if ($(checked).prop("checked") == true)
    //            {
                    
    //                $.ajax({
    //                    type: "POST",
    //                    url: "/BvFeedLogs/Preview",
    //                    data: {
    //                        //serviceTag: tdList[2].children[0].value,
    //                        //productId: tdList[3].children[0].value,
    //                        //locale: tdList[4].children[0].value,
    //                        //rating: tdList[5].children[0].value,
    //                        //reviewTitle: tdList[6].children[0].value,
    //                        //reviewText: tdList[7].children[0].value,
    //                        //nickName: tdList[8].children[0].value,
    //                        //email: tdList[9].children[0].value,
    //                        //isRecommended: tdList[10].children[0].value,
    //                        //isVerifiedBuyer: tdList[11].children[0].value,
    //                        //bvFeedConfigurationId: tdList[15].children[0].value,
    //                        //sessionId: tdList[16].children[0].value
    //                        bvFeedLogId: tdList[17].children[0].value
    //                    },

    //                    success: function (result) {
    //                        tdList[12].children[0].value = result.ErrorCode;
    //                        tdList[13].children[0].value = result.Response;
    //                        tdList[14].children[0].value = new Date().toLocaleString();
    //                    },
    //                    error: function (res) {
    //                        tdList[12].children[0].value = "";
    //                        tdList[13].children[0].value = res;
    //                        tdList[14].children[0].value = new Date().toLocaleString();
    //                    }
    //                });
    //            }
    //        }

    //    })

    //    $("#bv_submit").unbind("click").click(function (e) {
    //        e.preventDefault;
    //        var trList = $("#bv_feed_log_tbody").children("tr");
    //        for (var i = 0; i < trList.length; i++) {
    //            var tdList = trList.eq(i).find("td");
    //            $.ajax({
    //                type: "POST",
    //                url: "/BvFeedLogs/Submit",
    //                data: {
    //                    //serviceTag: tdList[2].children[0].value,
    //                    //productId: tdList[3].children[0].value,
    //                    //locale: tdList[4].children[0].value,
    //                    //rating: tdList[5].children[0].value,
    //                    //reviewTitle: tdList[6].children[0].value,
    //                    //reviewText: tdList[7].children[0].value,
    //                    //nickName: tdList[8].children[0].value,
    //                    //email: tdList[9].children[0].value,
    //                    //isRecommended: tdList[10].children[0].value,
    //                    //isVerifiedBuyer: tdList[11].children[0].value,
    //                    //bvFeedConfigurationId: tdList[15].children[0].value,
    //                    //sessionId: tdList[16].children[0].value
    //                    bvFeedLogId: tdList[17].children[0].value
    //                },
    //                success: function (result) {
    //                },
    //                error: function (res) {
    //                }
    //            });
    //        };
    //        window.location.href = "/BvFeedLogs/Index";
    //    })
    //}

    jq(function () {
        jq(":text:eq(0):visible").focus();

        //getBvFeedLogInfo();
    });

})(window.jQuery, window)

function Delete()
{
    $("#questionOptionstable").on("click", ".deleterow", function () {
        var parent = $(this).parent().parent().parent().parent().parent();
        parent.remove();
        ReplaceIndex();
    });
}


function ReplaceIndex() {
    var index = 0;
    var divLength = $("#questionOptionstable").children().length;
    for (var i = 0; i < divLength; i++) {

        var childTable = $("#questionOptionstable").children().eq(i).children();
        var tr = $("#questionOptionstable").children().eq(i).children().find("tr");
        var trLength = tr.length;
        for (var j = 1; j < trLength; j++) {
            childTable.children().children().eq(j).children().eq(0).children().attr('name', '[' + index + '].QuestionOptionsId');
            childTable.children().children().eq(j).children().eq(1).children().attr('name', '[' + index + '].DisplayOrder');
            childTable.children().children().eq(j).children().eq(2).children().eq(1).attr('name', '[' + index + '].OptionId');
            childTable.children().children().eq(j).children().eq(3).children().attr('name', '[' + index + '].NextQuestionId');
            index++;
        }
    }
}

function initializehide(count)
{
    for(var i = 2; i <= count; i++) {
        var initializeChildren = $("#questionOptionstable").children().eq(0).children().find("tr").eq(i).children();
        initializeChildren.eq(0).children().attr('style', 'display:none');
        initializeChildren.eq(1).children().attr('style', 'display:none');
    }
}

function addhide(count) {
    for (var i = 2; i <= count; i++) {
        var children = $("#hiddenHtml").children().eq(0).children().find("tr").eq(i).children();
        children.eq(0).children().attr('style','display:none');
        children.eq(1).children().attr('style','display:none');
    }
}

function addReplace(a)
{
    var html = $("#hiddenHtml").html();
    $("#hiddenHtml table tr").each(function () {
        html = html.replace('name="QuestionOptionsId"', 'name="[' + a + '].QuestionOptionsId"').
            replace('name="OptionId"', 'name="[' + a + '].OptionId"').
            replace('name="NextQuestionId"', 'name="[' + a + '].NextQuestionId"').
            replace('name="DisplayOrder"', 'name="[' + a + '].DisplayOrder"');
        a++;
    });
    $("#questionOptionstable").append(html);
}

function initializeElementHide(count)
{
    for(var i = 2; i <= count; i++) {
        var initializeChildren = $("#questionOptionstable").children().eq(0).children().find("tr");
        var val1 = initializeChildren.eq(i-1).children().eq(0).find('option:selected').val();
        var val2 = initializeChildren.eq(i).children().eq(0).find('option:selected').val();
        if(val1 == val2)
        {
            initializeChildren.eq(i).children().eq(0).children().attr('style', 'display:none');
            initializeChildren.eq(i).children().eq(1).children().attr('style', 'display:none');
            initializeChildren.eq(i).children().eq(1).children().attr('value', "");
        }

        else
        {
            continue;
        }
    }
}

function initializeDivHide(count)
{
    if (count == 0)
    {
        $("#questionOptionstable").children().remove();
    }
}


function addOption() {
    var index = $("#optiontable").children().length;
    var html = $("#hiddenHtml").html().replace('name="OptionId"', 'name="[' + index + '].OptionId"');
    html = html.replace('name="OptionDisplayTypeId"', 'name="[' + index + '].OptionDisplayTypeId"');
    html = html.replace('name="OptionDisplayOrderFake"', 'name="[' + index + '].DisplayOrder"');
    $("#optiontable").append(html);
}



function replace() {
    var index = 0;
    var divLength = $("#optiontable").children().length;
    for (var i = 0; i < divLength; i++) {
        var childTable = $("#optiontable").children().eq(i).children();
        childTable.eq(0).attr('name', '[' + index + '].OptionId');
        childTable.eq(1).attr('name', '[' + index + '].OptionDisplayTypeId');
        childTable.eq(2).attr('name', '[' + index + '].DisplayOrder');
        index++;
    }

}