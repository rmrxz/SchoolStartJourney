$(function () {
    var index = 0;
    $(document).on("click", "#singAndExchange", function (e) {
        e.preventDefault();
        singAnExchange();
    });
    singAnExchange();
    function singAnExchange() {
        $.ajax({
            cache: false,
            type: "get",
            async: true,
            url: "/ShoolStart/SingAndExchange?index=" + index,
            beforeSend: function () { }
        }).done(function (result) {
            if (result.ifLastIndex == true) {
                index = 0;
            }
            var html = "";
            for (var i = 0; i < result.items.length; i++) {
                html += '<li class="media-as" title=' + result.items[i].description + '>\
                            <div class="media-ph">\
                                <a href="#" class="an-detailied" data_Id="'+ result.items[i].id + '">\
                                    <img class="media-object thumbnail" src="' + result.items[i].avatar.uploadPath+'" alt="">\
                                </a>\
                            </div>\
                            <div class="media-con">\
                                <h4 class="media-heading"><a href="#" class="an-detailied" data_Id="'+ result.items[i].id + '">'+ result.items[i].name + '</a></h4>\
                                <h5>人数 '+ result.items[i].anAssociationNum + '</h5>\
                            </div>\
                       </li >';
            }
            $("#ulAnAssociation").html(html);
            index++;
            }).fail(function () {
                toastr.error("后台连接失败");
        }).always(function () {
            //最后操作的回调函数
        });
    }

    //查看用户信息
    $(document).on('click', '.seeUserDetailed', function (e) {
        e.preventDefault()
        window.location.href = "../../PersonalCenter/Index?id=" + $(this).attr("data_Id");
    });

    //点击查看社团明细
    $(document).on('click', '.an-detailied', function (e) {
        e.preventDefault();
        window.location.href = "../../AnAssociationView/Detailed/" + $(this).attr("data_Id");
    });

    //点击查看社团明细
    $(document).on('click', '.an-detailied', function (e) {
        e.preventDefault();
        window.location.href = "../../AnAssociationView/Detailed/" + $(this).attr("data_Id");
    });

    //点击查看社团明细
    $(document).on('click', '.activity-detailied', function (e) {
        e.preventDefault();
        window.location.href = "../../ActivityTermView/Detailied/" + $(this).attr("data_Id");
    });
})