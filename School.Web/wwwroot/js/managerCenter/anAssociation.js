
var isAnAssociationSearch = false;
$(function () {
    //筛选
    $(document).on('click', '.input-anAssociation-search', function (e) {
        e.preventDefault();
        isAnAssociationSearch = true;
        AnAssociationList();
    });

    //获取社团信息
    $(document).on('click', '.detailed-anAssociation', function (e) { 
        e.preventDefault();
        detailedAnAssociation($(this).attr("data-acId"))
    });
    $(document).on('click', '#detailed-anAssociation', function (e) {
        e.preventDefault();
        detailedAnAssociation($(this).attr("data-acId"))
    });

   
    //查看活动图片
    $(document).on('click', '#detailed-anAssociation-images', function (e) {
        e.preventDefault();
        detailedAnAssociationImages($(this).attr("data-acId"));
    })

    function detailedAnAssociationImages(id) {
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/AnAssociationDetailedImages/" +id,
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    };


    //查看活动成员
    $(document).on('click', '#detailed-anAssociation-members', function (e) {
        e.preventDefault();
        anAssociationMember($(this).attr("data-acId"));
    })

    function anAssociationMember(id) {
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/AnAssociationDetailedMenber/" + id,
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
            }).fail(function () {
                toastr.error("后台连接失败");
        }).always(function () {
        });
    }

    //删除社团成员
    $(document).on('click', '.delete-anAssociation-member', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/DeleteAnAssociationMember/" + $(this).attr("data-member-id"),
            beforeSend: function () { }
        }).done(function (result) {
            anAssociationMember($('#detailed-anAssociation-members').attr('data-acId'));
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });

    //点击解散社团
    $(document).on('click', '.dissolution-anAssociation', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/AnAssociationDissolution/" + $(this).attr("data-acId"),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                AnAssociationList();
                toastr.success(result.message);
            }
            else {
                toastr.warning(result.message)
            }
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });

    //点击退出社团
    $(document).on('click', '.out-anAssociation', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/OutAnAssociation/" + $(this).attr("data-acId"),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                toastr.success(result.message)
                AnAssociationList();
            }
            else {
                toastr.error(result.message)
            }
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });

    //触发input
    $(document).on('click', '.add-anAssociation-images', function () {
        $(".add-anAssociation-images-file").trigger("click");
    });

    //添加图片
    $(document).on('change', '.add-anAssociation-images-file', function (e) {
        var fileName = $(this).val();
        var suffixIndex = fileName.lastIndexOf(".");
        var suffix = fileName.substring(suffixIndex + 1).toUpperCase();
        if (suffix != "BMP" && suffix != "JPG" && suffix != "JPEG" && suffix != "PNG" && suffix != "GIF") {
            toastr.error("请上传图片（格式BMP、JPG、JPEG、PNG、GIF等）!");
            return;
        }
        var files = $('input[type=file]').get(0).files;

        //保存图片
        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append(files.name, files[i]);
        }
        var acId = $('#detailed-anAssociation-images').attr("data-acId");
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/SaveAnAssociationDetailedImages/' + acId,
            // 告诉jQuery不要去处理发送的数据
            processData: false,
            // 告诉jQuery不要去设置Content-Type请求头
            contentType: false,
            data: data,
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                detailedAnAssociationImages(acId);
            }
            else {
                toastr.error(result.message)
            }
            }).fail(function () {
                toastr.error("连接后台失败");
        }).always(function () {

        });

    });

    //删除图片
    $(document).on('click', '.delete-anAssociation-image', function (e) {
        e.preventDefault();
        var acId = $('#detailed-anAssociation-images').attr("data-acId");
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/DeleteAnAssociationDetailedImages?id=" + $(this).attr("data-image-Id") + "&&anAssociationId=" + acId,
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                detailedAnAssociationImages(acId);
            }
            else {
                toastr.error(result.message)
            }
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });

    //替换图片路径
    function readPath(file) {
        imaPath = $('.head-portrait img').attr("src");
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            $('#anAssociationImages').prepend('<li><a href="#" class="ac-image-op"><i class="fa fa-times"></i><img src="' + reader.result + '" alt=""> </a></li>');
        };
    }

})

// 社团列表
function AnAssociationList() {
    var keywork = $("#search-anAssociation").val();
    var listParaJson = nncqGetListParaJson();
    if ($('#nncqPageIndex').val() == undefined) {
        listParaJson = null;
    }
    if (isAnAssociationSearch) {
        listParaJson = null;
    }
    var jsonData = { "listPageParaJson": listParaJson };
    $.ajax({
        cache: false,
        type: 'post',
        async: false,
        url: "../../ManageCenter/AnAssociationList/?keywork=" + keywork,
        data: jsonData,
        beforeSend: function () { }
    }).done(function (result) {
        isAnAssociationSearch = false
        $('#anAssociationList').html(result);
    }).fail(function () {
        toastr.error("后台连接失败");
    }).always(function () {
    });
};
//社团明细
function detailedAnAssociation(id) {
    $.ajax({
        cache: false,
        type: 'post',
        async: false,
        url: "../../ManageCenter/AnAssociationDetailed/" + id,
        beforeSend: function () { }
    }).done(function (result) {
        $('#managerCenter').html(result);
    }).fail(function () {
        toastr.error("后台连接失败");
    }).always(function () {
    });
}
