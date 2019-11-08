$(function () {
    //触发input
    $(document).on('click', '.add-activity-images', function () {
        $(".add-ac-images-file").trigger("click");
    });

    //添加图片
    $(document).on('change', '.add-ac-images-file', function (e) {
        var fileName = $(this).val();
        var suffixIndex = fileName.lastIndexOf(".");
        var suffix = fileName.substring(suffixIndex + 1).toUpperCase();
        if (suffix != "BMP" && suffix != "JPG" && suffix != "JPEG" && suffix != "PNG" && suffix != "GIF") {
            toastr["info"]("请上传图片（格式BMP、JPG、JPEG、PNG、GIF等）!", "消息提示");
            return;
        }
        var files = $('input[type=file]').get(0).files;

        //保存图片
        var data = new FormData();
        for (var i = 0; i < files.length; i++) {
            data.append(files.name, files[i]);
        }
        var acId = $('#detailed-activity-images').attr("data-acId");
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/SaveActivityDetailedImages/' + acId,
            // 告诉jQuery不要去处理发送的数据
            processData: false,
            // 告诉jQuery不要去设置Content-Type请求头
            contentType: false,
            data: data,
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                detailedActivityImages(acId);
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
    $(document).on('click', '.delete-activity-image', function (e) {
        e.preventDefault();
        var acId = $('#detailed-activity-images').attr("data-acId");
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/DeleteActivityDetailedImages?id=" + $(this).attr("data-image-Id") + "&&activityId=" + acId,
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                detailedActivityImages(acId);
            }
            else {
                toastr.warning(result.message)
            }
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    });

    //重新加载图片
    function detailedActivityImages(id) {
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: "../../ManageCenter/ActivityDetailedImages/" + id,
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
        }).fail(function () {
            toastr.error("后台连接失败");
        }).always(function () {
        });
    }

    //替换图片路径
    function readPath(file) {
        imaPath = $('.head-portrait img').attr("src");
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            $('#activityImages').prepend('<li><a href="#" class="ac-image-op"><i class="fa fa-times"></i><img src="' + reader.result + '" alt=""> </a></li>');
        };
    }
});