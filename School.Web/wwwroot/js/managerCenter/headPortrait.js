$(function () {
    var imaPath;
    //触发input
    $(document).on('click', '.tn-head-portrait', function (e) {
        e.preventDefault();
        $(".head-file").trigger("click");
    });
    //选择图片
    $(document).on('change', '.head-file', function (e) {
        e.preventDefault();
        var fileName = $(this).val();
        var suffixIndex = fileName.lastIndexOf(".");
        var suffix = fileName.substring(suffixIndex + 1).toUpperCase();
        if (suffix != "BMP" && suffix != "JPG" && suffix != "JPEG" && suffix != "PNG" && suffix != "GIF") {
            toastr.error("请上传图片（格式BMP、JPG、JPEG、PNG、GIF等）!");
            return;
        }  
        $('.info-operation-bottom').html('<button class="tn-head-portrait" style="margin-right:4px">更改</button><button class="save-head-portrait">保存</button>')
        
        var file = $('input[type=file]').get(0).files[0];
        readPath(file);
    });

    //取消更换图片
    $(document).on('click', '.cancel-head-portrait', function (e) {
        e.preventDefault();
        $('.preview').html('');
        $('.info-operation-bottom').html(' <button class="tn-head-portrait">更换图片</button>');
        $('.head-portrait img').attr("src", imaPath)
    })
    //保存图片
    $(document).on('click', '.save-head-portrait', function (e) {
        e.preventDefault();
        var file = $('.head-file').get(0).files[0];
        var data = new FormData();
        data.append(file.name, file);
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/SaveHeadPortrait',
            // 告诉jQuery不要去处理发送的数据
            processData: false,
            // 告诉jQuery不要去设置Content-Type请求头
            contentType: false,
            data: data,
            
            beforeSend: function () { }
        }).done(function (result) {
            UserDetail();         
            $('.preview').html('');
            $('.info-operation-bottom').html(' <button class="tn-head-portrait">更换图片</button>');
            }).fail(function () {
                toastr.error("连接后台失败");
        }).always(function () {

        });
    })

    //点击图片放大
    $(document).on('click','.head-portrait',function(e)
    {
        e.preventDefault();
        var heightWingow=$(window).height();
        $('.Mask').css({"height":heightWingow*0.75,"display":"block","paddingTop":heightWingow*0.25});
        $('.mask-image img').css({"height":heightWingow*0.5,"width":heightWingow*0.5});
        $('.mask-image img').attr("src",$(this).children("img").attr("src"));
    });

    //点击关闭大图
    $(document).on('click','.Mask',function(e)
    {
        e.preventDefault();
        var heightWingow=$(window).height();
        $('.Mask').removeAttr("style");
        $('.mask-image img').removeAttr("style");
    });


    //替换图片路径
    function readPath(fil) {
        imaPath = $('.head-portrait img').attr("src");
        var reader = new FileReader();
        reader.readAsDataURL(fil);
        reader.onload = function () {
            $('.head-portrait img').attr("src", reader.result);
            $('.preview').html('<a href="#" class="cancel-head-portrait"><i class="fa fa-times"></i></a><img src="' + reader.result + '" alt= "" >');
        };
    }
});
