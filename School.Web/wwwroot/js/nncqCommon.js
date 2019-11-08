/*
 *  根据指定的地址直接链接跳转回退
 * @param {} urlString 
 * @returns {} 
 */
function nncqGotoNewPage(urlString) {
    window.location.href = urlString;
}

/*
 *  根据指定的地址 urlString 访问控制器方法，然后根据返回的局部页刷新指定的 targetDiv 区域
 * @param {} urlString 
 * @param {} targetDivElelmentID 
 * @returns {} 
 */
function nncqGotoNewPartial(urlString, targetDivElelmentID) {
    nncqGotoNewPartialByJsonAndShowStatus(urlString, "", targetDivElelmentID, "", true);
}


/*
 * 根据指定的地址 urlString 访问控制器方法，
 * 执行访问时，在指定的位置呈现状态信息，
 * 然后根据返回的局部页刷新指定的 targetDiv 区域
 * @param {} urlString 
 * @param {} targetDivElelmentID 
 * @param {} statucMessage 
 * @returns {} 
 */
function nncqGotoNewPartialAndShowStatus(urlString, targetDivElelmentID, statucMessage) {
    nncqGotoNewPartialByJsonAndShowStatus(urlString, "", targetDivElelmentID, statucMessage, true);
}

/*
 * 根据指定的地址 urlString 和 jsonData 访问控制器方法，
 * 
 * @param {} urlString 
 * @param {} jsonData 
 * @param {} targetDivElelmentID 
 * @returns {} 
 */
function nncqGotoNewPartialByJson(urlString, jsonData, targetDivElelmentID) {
    nncqGotoNewPartialByJsonAndShowStatus(urlString, jsonData, targetDivElelmentID, "", true);
}

/*
 * 根据指定的地址 urlString 和 jsonData 访问控制器方法,
 * 执行访问时，在指定的位置呈现状态信息，
 * 然后根据返回的局部页刷新指定的 targetDivElelmentID 区域
 * 
 * @param {} urlString 
 * @param {} jsonData 
 * @param {} targetDivElelmentID 
 * @param {} statusMessage
 * * @param {} isAsync 
 * @returns {} 
 */
function nncqGotoNewPartialByJsonAndShowStatus(urlString, jsonData, targetDivElelmentID, statusMessage, isAsync) {
    $.ajax({
        cache: false,
        type: "POST",
        async: isAsync,
        url: urlString,
        data: jsonData,
        beforeSend: function () {
            if (statusMessage !== "") {
                $("#" + targetDivElelmentID).html(statusMessage);
            }
        }
    }).done(function (data) {
        var reg = /^<script>.*<\/script>$/;
        if (reg.test(data)) {
            // 这句是为了响应后台返回的js
            $('body').append("<span id='responseJs'>" + data + "</span>").remove("#responseJs");
            return;
        } else {
            if (targetDivElelmentID !== '') {
                $("#" + targetDivElelmentID).html(data);
            }
        }
    }).fail(function (jqXHR, textStatus, errorThrown) {
        console.error("调试错误:" + errorThrown);
    }).always(function () {
    });
}

/*
 * 创建新的 ListParaJson 对象
 * @param {} typeID 
 * @returns {} 
 */
function nncqCreateListParaJson(typeID) {
    nncqIntializationListPageParameter(typeID);
    var listParaJson = nncqGetListParaJson();
    return listParaJson;
}

/*
 *  重新初始化页面规格参数
 * @param {} typeID 
 * @returns {} 
 */
function nncqIntializationListPageParameter(typeID) {
    $("#nncqTypeID").val(typeID);
    $("#nncqPageIndex").val("1");
    $("#nncqPageSize").val("18");
    $("#nncqPageAmount").val("0");
    $("#nncqObjectAmount").val("0");
    $("#nncqKeyword").val("");
    $("#nncqSortProperty").val("SortCode");
    $("#nncqSortDesc").val("default");
    $("#nncqSelectedObjectID").val("");
    $("#nncqIsSearch").val("False");
}

/*
 * 提取页面分页规格数据,构建 ListParaJson 对象
 * @returns {} 
 */
function nncqGetListParaJson() {
    // 提取缺省的页面规格参数
    var nncqPageTypeID = $("#nncqTypeID").val();
    var nncqPagePageIndex = $("#nncqPageIndex").val();
    var nncqPagePageSize = $("#nncqPageSize").val();
    var nncqPagePageAmount = $("#nncqPageAmount").val();
    var nncqPageObjectAmount = $("#nncqObjectAmount").val();
    var nncqPageKeyword = $("#nncqKeyword").val();
    var nncqPageSortProperty = $("#nncqSortProperty").val();
    var nncqPageSortDesc = $("#nncqSortDesc").val();
    var nncqPageSelectedObjectID = $("#nncqSelectedObjectID").val();
    var nncqPageIsSearch = $("#nncqIsSearch").val();
    // 创建前端 json 数据对象
    var listParaJson = "{" +
        "ObjectTypeID:\"" + nncqPageTypeID + "\", " +
        "PageIndex:\"" + nncqPagePageIndex + "\", " +
        "PageSize:\"" + nncqPagePageSize + "\", " +
        "PageAmount:\"" + nncqPagePageAmount + "\", " +
        "ObjectAmount:\"" + nncqPageObjectAmount + "\", " +
        "Keyword:\"" + nncqPageKeyword + "\", " +
        "SortProperty:\"" + nncqPageSortProperty + "\", " +
        "SortDesc:\"" + nncqPageSortDesc + "\", " +
        "IsSearch:\"" + nncqPageIsSearch + "\", " +
        "SelectedObjectID:\"" + nncqPageSelectedObjectID + "\"" +
        "}";

    return listParaJson;
}