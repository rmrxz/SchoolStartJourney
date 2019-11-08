$(function () {
    ////取消a标签的默认点击事件
    //$(document).on('click', 'a', function (e) {
    //    e.preventDefault();
    //});
    //查看用户信息
    $(document).on('click', '.seeUserDetailed', function (e) {
        e.preventDefault()
        window.location.href = "../../PersonalCenter/Index?id=" + $(this).attr("data_Id");
    })
    userInfo();
    UserDetail();
    messageNotification();
    function userInfo() {
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/UserInfo',
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    };
    //信息设置
    $(document).on('click', '.Index', function (e) {
        e.preventDefault();
        userInfo();
    });

    //爱好设置
    $(document).on('click', '.Hobby', function (e) {
        e.preventDefault();
        Hobby();
    });

    function Hobby() {
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/UserHobbys',
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    }

    //保存爱好 
    $(document).on('click', '.save-user-hobby', function (e) {
        e.preventDefault();
        var user_hobby_input = $('.user_hobby input');
        var hobbys = []
        for (var i = 0; i < user_hobby_input.length; i++) {
            hobbys.push(user_hobby_input.eq(i).val());
        }
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/UpdateUserHobbys?hobbys=' + hobbys,
            beforeSend: function () { }
        }).done(function (result) {
            if (result) {
                toastr.success("保存成功");
            }
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    });

    //======================
    /*爱好*/
    //======================
    $(document).on('click', '#ho-choose li', function () {
        $('#ho-see ul').append('  <li class="hoddy-li-a">' +
            '<img src="' + $(this).children().attr("data_img") + '" alt="">' +
            '<div class="ho-ma"><input type="hidden" name="' + $(this).children().text() + '" value="' + $(this).children().attr("hobby_Id") + '"  data_img="' + $(this).children().attr("data_img") + '">' +
            '<h5>' + $(this).children().text() + '</h5>' +
            '<a href="#" class="delete-hobby">删除</a>' +
            '</div>' +
            '</li>');
        $(this).remove();
    })

    $(document).on('click', '.delete-hobby', function () {
        $(this).parents('.hoddy-li-a').remove();
        $('#ho-choose ul').append(' <li><label hobby_Id="' + $(this).prevAll("input").val() + '"  data_img="' + $(this).prevAll("input").attr('data_img') + '">' + $(this).prevAll("input").attr("name") + '</label></li>')
    });

    //点击我的好友
    $(document).on('click', '.userFriends', function (e) {
        e.preventDefault();
        friends();
    });

    //删除好友
    $(document).on('click', '.delete-user-friend', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/DeleteFriend?id=' + $(this).attr('data-user-id'),
            beforeSend: function () { }
        }).done(function (result) {
            if (result.isOK) {
                friends();
            }
            else {
                toastr.error(result.message)
            }
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    });

    function friends() {
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/UserFriends',
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
            ActivityList();
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    }

    //通知界面
    $(document).on('click', '.messageNotification', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/GetNotification',
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
            meeageNotificationList()
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });

    });


    //点击头像设置
    $(document).on('click', '.headPortrait', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/HeadPortrait',
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    });

    //点击我的活动
    $(document).on('click', '.activity', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/Activity',
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
            ActivityList();
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    });

    //点击创建活动
    $(document).on('click', '.addActivity', function (e) {
        e.preventDefault();
        CreateOrEditActivity("");
    });

    //点击编辑活动
    $(document).on('click', '.editActivity', function (e) {
        e.preventDefault();
        var id = $(this).attr("data-acId");
        CreateOrEditActivity(id);

    });

    //点击我的社团
    $(document).on('click', '.anAssociation', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/AnAssociation',
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
            AnAssociationList();
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    });

    setInterval(function () {
        messageNotification();
    }, 10000);

    function CreateOrEditActivity(id) {
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/AddActivity?Id=' + id,
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    }


    //点击创建社团
    $(document).on('click', '.addAnAssociation', function (e) {
        e.preventDefault();
        CreateOrEditAnAssociation("");
    });

    //点击编辑社团
    $(document).on('click', '.editAnAssociation', function (e) {
        e.preventDefault();
        var id = $(this).attr("data-acId");
        CreateOrEditAnAssociation(id);

    });


    function CreateOrEditAnAssociation(id) {
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/AddOrEditAnAssociation?Id=' + id,
            beforeSend: function () { }
        }).done(function (result) {
            $('#managerCenter').html(result);
        }).fail(function () {
            toastr.error("连接后台失败");
        }).always(function () {

        });
    }

    //新增或更新社团保存事件
    $(document).on('click', '.save-anAssociationForm', function (e) {
        e.preventDefault();
        var data = $('form[name=anAssociationForm]').serializeArray();
        var dataString = "";
        $.each(data, function (i, obj) {
            dataString += obj.name + ":";
            dataString += '"' + obj.value + '",';
        });
        var jsonData = { "anAssociationData": "{" + dataString + "}" }
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/SaveAnAssociation',
            data: jsonData,
            beforeSend: function () { }
        }).done(function (data) {
            if (!data.result) {
                toastr.error(data.message);
            }
            else {
                detailedAnAssociation(data.id);
                toastr.success(data.message);
            }
        }).fail(function () {
            toastr.error("后台连接失败")
        }).always(function () {

        });
    });

    $(document).on('click', '.user-Password', function (e) {
        e.preventDefault();
        $.ajax({
            cache: false,
            type: 'post',
            async: false,
            url: '../../ManageCenter/UserPassword',
            beforeSend() { }
        }).done(function (result) {
            $('#managerCenter').html(result);
        }).fail(function () {
            toastr.error("连接后台失败");
        })
    })
});
function UserDetail() {
    $.ajax({
        cache: false,
        type: 'post',
        async: false,
        url: "../../ManageCenter/UserDetail",
        beforeSend: function () { }
    }).done(function (result) {
        //console.log(result);
        $('#userDetail').html(result);
    }).fail(function () {
        toastr.error("后台连接失败");
    }).always(function () {
    });
};
$(function () {
    toastr.options = {
        closeButton: false,
        debug: false,
        progressBar: true,
        positionClass: "toast-bottom-right",
        onclick: null,
        showDuration: "300",
        hideDuration: "1000",
        timeOut: "2000",
        extendedTimeOut: "1000",
        showEasing: "swing",
        hideEasing: "linear",
        showMethod: "fadeIn",
        hideMethod: "fadeOut"
    };
    if ($('#messageNotificationNumber').text() > 0) {
        toastr.success("你有" + $('#messageNotificationNumber').text() + "消息未查看");
    }
});

